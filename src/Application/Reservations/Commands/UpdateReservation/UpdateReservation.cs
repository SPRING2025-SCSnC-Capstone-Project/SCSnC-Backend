using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Reservations;

public record UpdateReservationCommand : IRequest<ReservationDto> {
    public Guid Id { get; init; }
    public string PaymentMethod { get; init; }
}

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;

    public UpdateReservationCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
    }

    public async Task<ReservationDto> Handle(UpdateReservationCommand request, CancellationToken cancellationToken) {
        var reservation = await _context.Reservations
            .Include(x => x.ReservedSlots)
                .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (reservation is null) {
            throw new KeyNotFoundException($"Reservation with Id {request.Id} does not exist");
        }

        var now = DateTime.UtcNow;
        var instant = Instant.FromDateTimeUtc(now);
        var timeZone = DateTimeZoneProviders.Tzdb["Asia/Ho_Chi_Minh"];
        var zonedDateTime = instant.InZone(timeZone);
        var localDateTime = zonedDateTime.LocalDateTime;

        var reservationDate = reservation.ReserveDate;
        var reservationSlots = reservation.ReservedSlots.Count > 0 ? reservation.ReservedSlots.OrderBy(x => x.Slot.SlotNumber).ToList() : new List<Domain.Entities.ReservedSlot>();
        var reservationStartTime = reservationSlots.Count > 0 ? reservationSlots[0].Slot.TimeStart : reservation.TimeStart.Value;
        var reservationStartDate = new LocalDateTime(reservationDate.Year, reservationDate.Month, reservationDate.Day, reservationStartTime.Hour, reservationStartTime.Minute, reservationStartTime.Second);
        var reservationEndTime = reservationSlots.Count > 0 ? reservationSlots[reservationSlots.Count - 1].Slot.TimeEnd : reservation.TimeEnd.Value;
        var reservationEndDate = new LocalDateTime(reservationDate.Year, reservationDate.Month, reservationDate.Day, reservationEndTime.Hour, reservationEndTime.Minute, reservationEndTime.Second);

        if(reservationEndDate <= localDateTime || reservation.IsCanceled == true || reservation.Status.ToLower().Equals("done") || localDateTime < reservationStartDate)
        {
            throw new RequestValidationException("Không thể thanh toán");
        }

        if (request.PaymentMethod.ToLower().Equals("cash"))
        {
            reservation.IsFullPaid = true;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var returnReservationData = _mapper.Map<ReservationDto>(reservation);

        switch (request.PaymentMethod)
        {
            case "VNPay":
                VNPayRequest vnPayRequest = new VNPayRequest()
                {
                    vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
                    vnp_Amount = (decimal)(reservation.TotalPrice - reservation.Deposit) * 100,
                    vnp_OrderType = "other",
                    vnp_OrderInfo = $"Date: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Total Price: {reservation.TotalPrice}",
                    vnp_TxnRef = $"{reservation.Id.ToString()}_lmao",
                    vnp_Command = "pay",
                    vnp_ExpireDate = DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"),
                };
                var paymentUrl = await _vnpayService.GetPaymentLink(vnPayRequest);
                returnReservationData.PaymentLink = paymentUrl;

                break;
        }



        return returnReservationData;
    }
}
