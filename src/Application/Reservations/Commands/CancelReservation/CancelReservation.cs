using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using System.Diagnostics;

namespace Application.Reservations.Commands;

public record CancelReservationCommand : IRequest<ResponseReservationDto> {
    public Guid ReservationId { get; set; }
    public Guid UserId { get; set; }
}

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ResponseReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _vnpayService;

    public CancelReservationCommandHandler(IMapper mapper, IApplicationDbContext context, IPaymentService vnpayService)
    {
        _mapper = mapper;
        _context = context;
        _vnpayService = vnpayService;
    }

    public async Task<ResponseReservationDto> Handle(CancelReservationCommand request, CancellationToken cancellationToken) {
            var today = LocalDate.FromDateTime(DateTime.UtcNow);
            var user = _context.Users.First(x => x.Id.Equals(request.UserId));
            var canceledReservation = await _context.Reservations.Where(x => x.UserId.Equals(request.UserId) && x.LastUpdatedAt.Date.Equals(today) && x.IsCanceled == true && x.Status.ToLower().Equals("canceled")).ToListAsync();
            if(user.Role.ToLower().Equals("customer") && canceledReservation.Count >= 2)
            {
                throw new KeyNotFoundException($"Chỉ được hủy đặt phòng 2 lần trong 1 ngày");
            }
            var reservationTransaction = await _context.Transactions.Include(x => x.Reservation).ThenInclude(y => y.Event).Include(x => x.Order).FirstOrDefaultAsync(x => x.Reservation.Id.Equals(request.ReservationId));
            reservationTransaction.Reservation.IsFullPaid = false;
            reservationTransaction.Reservation.Status = "Canceled";
            reservationTransaction.Reservation.IsCanceled = true;
            reservationTransaction.Reservation.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
            if (reservationTransaction.Reservation.Event != null)
            {
                reservationTransaction.Reservation.Event.IsActive = false;
                reservationTransaction.Reservation.Event.IsCanceled = true;
                reservationTransaction.Reservation.Event.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
            }
            if (reservationTransaction.Order != null)
            {
                reservationTransaction.Order.IsActive = false;
                reservationTransaction.Order.PaymentStatus = false;
                reservationTransaction.Order.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
            }
            reservationTransaction.TransactionStatus = "Failed";
            _context.Transactions.Update(reservationTransaction);

            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ResponseReservationDto>(reservationTransaction.Reservation);
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
