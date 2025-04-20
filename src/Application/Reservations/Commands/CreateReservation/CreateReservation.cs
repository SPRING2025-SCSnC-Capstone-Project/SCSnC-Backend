using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;

namespace Application.Reservations.Commands;

public record CreateReservationCommand : IRequest<ResponseReservationDto> {
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceId { get; init; }
    public double Deposit { get; init; }
    public Guid? UserId { get; init; }
    public string PhoneNumber { get; init; } = null!;
    public string Email { get; init; } = null!;
    public double TotalPrice { get; init; }
    public Guid[] SlotIds { get; init; } = null!;
    public Guid[]? WorkspaceUtilityServiceIds { get; set; } 
    public string PaymentMethod { get; init; } = null!;
}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ResponseReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;

    public CreateReservationCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
    }

    public async Task<ResponseReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == request.WorkspaceId && x.IsActive, cancellationToken);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null) {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        var reservedSlotsToAdd = new List<ReservedSlot>(); 
        var reservationUtilityServicesToAdd = new List<ReservationUtilityService>();

        foreach (var slotId in request.SlotIds) {
            var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == slotId 
                    && x.IsActive, cancellationToken);
            
            if (slot is null) {
                throw new KeyNotFoundException($"Slot with Id {slotId} does not exist");
            }

            if (await CheckConflict(request.ReservationDate, slotId, cancellationToken)) {
                throw new ConflictException("One or more slots have already been reserved");
            }
        }

        if (request.WorkspaceUtilityServiceIds != null) {
            foreach (var workspaceUtilityServiceId in request.WorkspaceUtilityServiceIds) {
                var workspaceUtilityService = await _context.WorkspaceUtilityServices.FirstOrDefaultAsync(
                    x => x.Id == workspaceUtilityServiceId, cancellationToken
                );

                if (workspaceUtilityService is null) {
                    throw new KeyNotFoundException($"Workspace utility service with Id {workspaceUtilityServiceId} does not exist");
                }
            }
        }

        var reservation = new Reservation() {
            UserId = request.UserId,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            WorkspaceId = request.WorkspaceId,
            Deposit = request.Deposit,
            IsFullPaid = false,
            TotalPrice = request.TotalPrice,
            ReserveDate = LocalDate.FromDateOnly(request.ReservationDate),
            Status = "Pending",
        };

        var result = await _context.Reservations.AddAsync(reservation, cancellationToken);

        foreach (var slotId in request.SlotIds) {
            var reservedSlot = new ReservedSlot() {
                SlotId = slotId,
                ReservationId = result.Entity.Id,
            };
            
            reservedSlotsToAdd.Add(reservedSlot);
        };

        await _context.ReservedSlots.AddRangeAsync(reservedSlotsToAdd, cancellationToken);

        foreach (var workspaceUtilityServiceId in request.WorkspaceUtilityServiceIds) {
            var reservationUtilityService = new ReservationUtilityService() {
                ReservationId = result.Entity.Id,
                WorkspaceUtilityServiceId = workspaceUtilityServiceId,
            };


            reservationUtilityServicesToAdd.Add(reservationUtilityService);
        }

        await _context.ReservationUtilityServices.AddRangeAsync(reservationUtilityServicesToAdd, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var createdReservation = await _context.Reservations
            .Include(x => x.Workspace)
            .ThenInclude(y => y.WorkspaceType)
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .Include(x => x.ReservationUtilityServices)
            .ThenInclude(y => y.WorkspaceUtilityService)
            .ThenInclude(z => z.UtilityService)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);

        var returnReservationData = _mapper.Map<ResponseReservationDto>(createdReservation);

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

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken) {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x => 
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate) 
                && (x.Reservation.Status.Equals("Pending") || x.Reservation.Status.Equals("Booked"))
                && x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }
}
