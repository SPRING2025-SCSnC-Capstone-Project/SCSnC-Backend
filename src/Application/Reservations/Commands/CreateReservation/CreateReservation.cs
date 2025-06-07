using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Application.Reservations.Commands;

public record CreateReservationCommand : IRequest<ResponseReservationDto>
{
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceTypeId { get; set; }
    public Guid WorkspaceId { get; init; }
    public double Deposit { get; init; }
    public Guid? UserId { get; init; }
    public string PhoneNumber { get; init; } = null!;
    public double TotalPrice { get; init; }
    public Guid[]? SlotIds { get; init; } = null!;
    public string? Note { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    //public DateTimeOffset startDate { get; set; }
    //public DateTimeOffset endDate { get; set; }
    public bool includeEvent { get; init; } = false;
    public string? EventTitle { get; init; } = null!;
    public string? EventDescription { get; init; } = null!;
    public string? CoverImageLink { get; init; }
    public double? EntranceFee { get; init; }
    public string PaymentMethod { get; init; }
    public bool IsEventPrivate { get; init; } = false;
    public Guid BranchId { get; init; }
    public TimeOnly TimeStart { get; init; }
    public TimeOnly TimeEnd { get; init; }
    public bool BookingWithTime { get; init; }
    public Guid[]? WorkspaceUtilityServiceIds { get; set; }
    public IFormFile? File { get; set; }

}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ResponseReservationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;
    private readonly IAzureService _azureService;

    public CreateReservationCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService, IAzureService azureService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
        _azureService = azureService;
    }

    public async Task<ResponseReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == request.WorkspaceId && x.IsActive, cancellationToken);
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);
        var bookedReservation = _context.Reservations.Where(x => x.UserId.Equals(request.UserId) && x.Status.ToLower().Equals("booked")).ToList();

        if (workspace is null)
        {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null)
        {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        if (user.Role.ToLower().Equals("customer") && bookedReservation.Count >= 2)
        {
            throw new KeyNotFoundException($"Chỉ được đặt phòng trước 2 lần");
        }

        var reservedSlotsToAdd = new List<ReservedSlot>();
        var reservationUtilityServicesToAdd = new List<ReservationUtilityService>();

        if (request.SlotIds != null && request.SlotIds.Length > 0)
        {
            foreach (var slotId in request.SlotIds)
            {
                var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == slotId && x.IsActive, cancellationToken);

                if (slot is null)
                {
                    throw new KeyNotFoundException($"Slot with Id {slotId} does not exist");
                }

                //if (await CheckConflict(request.ReservationDate, slotId, cancellationToken))
                //{
                //    throw new ConflictException("One or more slots have already been reserved");
                //}
            }
        }

        if (request.WorkspaceUtilityServiceIds != null)
        {
            foreach (var workspaceUtilityServiceId in request.WorkspaceUtilityServiceIds)
            {
                var workspaceUtilityService = await _context.WorkspaceUtilityServices.FirstOrDefaultAsync(
                    x => x.Id == workspaceUtilityServiceId, cancellationToken
                );

                if (workspaceUtilityService is null)
                {
                    throw new KeyNotFoundException($"Workspace utility service with Id {workspaceUtilityServiceId} does not exist");
                }
            }
        }
        Debug.WriteLine(request.BranchId);
        var reservation = new Reservation()
        {
            UserId = request.UserId,
            WorkspaceId = request.WorkspaceId,
            Deposit = request.Deposit,
            IsFullPaid = request.TotalPrice == request.Deposit,
            TotalPrice = request.TotalPrice,
            ReserveDate = LocalDate.FromDateOnly(request.ReservationDate),
            Email = request.Email,
            Phone = request.Phone,
            Note = request.Note,
            //EndDate = request.endDate,
            //StartDate = request.startDate,
            BranchId = request.BranchId,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            TimeStart = LocalTime.FromTimeOnly(request.TimeStart),
            TimeEnd = LocalTime.FromTimeOnly(request.TimeEnd),
            Status = request.PaymentMethod.ToLower().Equals("cash") ? "Booked" : "Pending"
        };
        Event reservationEvent = new Event();
        var newReservation = await _context.Reservations.AddAsync(reservation, cancellationToken);
        var imageUrl = "";

        if (request.includeEvent)
        {
            var entity = new Event()
            {
                EventTitle = request.EventTitle,
                EventDescription = request.EventDescription ?? "",
                EventDate = reservation.ReserveDate,
                EntranceFee = request.EntranceFee ?? 0,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                ReservationId = newReservation.Entity.Id,
                IsActive = false,
                IsPrivate = request.IsEventPrivate,
                IsCanceled = false
            };

            if (request.File != null)
            {
                imageUrl = await _azureService.UploadFile(request.File, $"{request.EventTitle + Guid.NewGuid()}.png");
            }
            entity.CoverImageLink = imageUrl;

            var result = await _context.Events.AddAsync(entity, cancellationToken);

            if (!request.BookingWithTime)
            {
                var eventSlotsToAdd = new List<EventSlot>();

                foreach (var slotId in request.SlotIds)
                {
                    var eventSlot = new EventSlot()
                    {
                        SlotId = slotId,
                        EventId = result.Entity.Id,
                    };

                    eventSlotsToAdd.Add(eventSlot);
                }

                await _context.EventSlots.AddRangeAsync(eventSlotsToAdd, cancellationToken);
            }

            var added_event = await _context.Events
                .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceTypeAtBranch)
                    .ThenInclude(w => w.WorkspaceType)
                .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceTypeAtBranch)
                    .ThenInclude(w => w.Branch)
                .AsNoTracking()
                .Include(x => x.Reservation)
                    .ThenInclude(y => y.User)
                .Include(x => x.EventSlots)
                    .ThenInclude(y => y.Slot)
                .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);
            await _context.Events.AddAsync(entity, cancellationToken);
            reservationEvent = added_event;
        }

        if (!request.BookingWithTime)
        {
            foreach (var slotId in request.SlotIds)
            {
                var reservedSlot = new ReservedSlot()
                {
                    SlotId = slotId,
                    ReservationId = newReservation.Entity.Id,
                };

                reservedSlotsToAdd.Add(reservedSlot);
            };
            await _context.ReservedSlots.AddRangeAsync(reservedSlotsToAdd, cancellationToken);
        }

        if(request.WorkspaceUtilityServiceIds != null)
        {
            foreach (var workspaceUtilityServiceId in request.WorkspaceUtilityServiceIds)
            {
                var reservationUtilityService = new ReservationUtilityService()
                {
                    ReservationId = newReservation.Entity.Id,
                    WorkspaceUtilityServiceId = workspaceUtilityServiceId,
                };
                reservationUtilityServicesToAdd.Add(reservationUtilityService);
            }
            await _context.ReservationUtilityServices.AddRangeAsync(reservationUtilityServicesToAdd, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        var createdReservation = await _context.Reservations
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.Branch)
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
                .ThenInclude(y => y.Slot)
            .Include(x => x.ReservationUtilityServices)
            .ThenInclude(y => y.WorkspaceUtilityService)
            .ThenInclude(z => z.UtilityService)
            .FirstOrDefaultAsync(x => x.Id == newReservation.Entity.Id, cancellationToken);

        var returnReservationData = _mapper.Map<ResponseReservationDto>(createdReservation);
        returnReservationData.Event = reservationEvent;

        switch (request.PaymentMethod)
        {
            case "VNPay":
                VNPayRequest vnPayRequest = new VNPayRequest()
                {
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
                    vnp_Amount = (decimal)createdReservation!.Deposit * 100,
                    vnp_OrderType = "other",
                    vnp_OrderInfo = $"Date: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Total Price: {createdReservation.Deposit}",
                    vnp_TxnRef = createdReservation.Id.ToString(),
                    vnp_Command = "pay",
                    vnp_ExpireDate = DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"),
                };
                var paymentUrl = await _vnpayService.GetPaymentLink(vnPayRequest);
                returnReservationData.PaymentLink = paymentUrl;

                var statusVNPay = await PaymentHelper.CreateTransaction(null, returnReservationData.Id, createdReservation.Deposit,
                    request.PaymentMethod, _context, cancellationToken);

                if (statusVNPay.IsSuccess)
                {
                    break;
                }
                throw new Exception(statusVNPay.Message);

            case "Cash":
                returnReservationData.PaymentLink = "Please pay at the cashier in the next 15 minutes. If no confirmation is received, the reservation will be canceled.";

                var statusCash = await PaymentHelper.CreateTransaction(null, returnReservationData.Id, createdReservation.Deposit,
                    request.PaymentMethod, _context, cancellationToken);

                if (statusCash.IsSuccess)
                {
                    break;
                }
                throw new Exception(statusCash.Message);
        }

        return returnReservationData;
    }

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken)
    {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x =>
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate)
                && (x.Reservation.Status.Equals("Pending") || x.Reservation.Status.Equals("Booked"))
                && x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }

}
