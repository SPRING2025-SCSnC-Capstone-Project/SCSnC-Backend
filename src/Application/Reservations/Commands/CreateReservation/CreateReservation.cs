using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using System.Diagnostics;

namespace Application.Reservations.Commands;

public record CreateReservationCommand : IRequest<ReservationDto>
{
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceTypeId { get; set; }
    public Guid WorkspaceId { get; init; }
    public double Deposit { get; init; }
    public Guid UserId { get; init; }
    public double TotalPrice { get; init; }
    public Guid[] SlotIds { get; init; } = null!;
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

}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;

    public CreateReservationCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.Id == request.WorkspaceTypeId && x.IsActive, cancellationToken);

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        if (workspaceType is null)
        {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null)
        {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        var reservedSlotsToAdd = new List<ReservedSlot>();

        foreach (var slotId in request.SlotIds)
        {
            var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == slotId && x.IsActive, cancellationToken);

            if (slot is null)
            {
                throw new KeyNotFoundException($"Slot with Id {slotId} does not exist");
            }

            if (await CheckConflict(request.ReservationDate, slotId, cancellationToken))
            {
                throw new ConflictException("One or more slots have already been reserved");
            }
        }
        var reservation = new Reservation()
        {
            UserId = request.UserId,
            WorkspaceId = request.WorkspaceId,
            Deposit = request.TotalPrice,
            IsFullPaid = true,
            TotalPrice = request.TotalPrice,
            ReserveDate = LocalDate.FromDateOnly(request.ReservationDate),
            Email = request.Email,
            Phone = request.Phone,
            Note = request.Note,
            //EndDate = request.endDate,
            //StartDate = request.startDate,
        };
        Event reservationEvent = new Event();
        var newReservation = await _context.Reservations.AddAsync(reservation, cancellationToken);

        if (request.includeEvent)
        {
            var entity = new Event()
            {
                EventTitle = request.EventTitle!,
                EventDescription = request.EventDescription!,
                //EventDate = reservation.ReserveDate.ToOffsetDateTime().Date,
                CoverImageLink = "",
                EntranceFee = request.EntranceFee!.Value,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                ReservationId = reservation.Id,
                IsActive = true,
                Status = "Accepted",
            };
            await _context.Events.AddAsync(entity, cancellationToken);
            reservationEvent = entity;
        }

        Debug.WriteLine(request.SlotIds[1]);

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

        await _context.SaveChangesAsync(cancellationToken);

        var createdReservation = await _context.Reservations
            .Include(x => x.Workspace)
            .ThenInclude(y => y.WorkspaceType)
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == newReservation.Entity.Id, cancellationToken);

        VNPayConfig vnPayConfig = VNPayHelper.GetConfigData();
        VNPayRequest vnPayRequest = new VNPayRequest()
        {
            vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
            vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
            vnp_Amount = (decimal)createdReservation.TotalPrice * 100,
            vnp_OrderType = "other",
            vnp_OrderInfo = $"Date: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Total Price: {createdReservation.TotalPrice}",
            vnp_TxnRef = $"reservation:{createdReservation.Id}",
            vnp_Command = "pay",
            vnp_ReturnUrl = vnPayConfig.ReturnUrl,
            vnp_ExpireDate = DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"),
        };

        var paymentUrl = await _vnpayService.GetPaymentLink(vnPayRequest);

        var result = _mapper.Map<ReservationDto>(createdReservation);
        result.Event = reservationEvent;
        result.PaymentLink = paymentUrl;

        return result;
    }

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken)
    {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x =>
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate) &&
                x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }

}
