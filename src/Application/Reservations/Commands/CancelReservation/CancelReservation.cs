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
        try
        {
            var reservationTransaction = await _context.Transactions.Include(x => x.Reservation).ThenInclude(y => y.Event).Include(x => x.Order).FirstOrDefaultAsync(x => x.Reservation.Id.Equals(request.ReservationId));
            reservationTransaction.Reservation.IsFullPaid = false;
            if (reservationTransaction.Reservation.Event != null)
            {
                reservationTransaction.Reservation.Event.IsActive = false;
            }
            if (reservationTransaction.Order != null)
            {
                reservationTransaction.Order.IsActive = false;
                reservationTransaction.Order.PaymentStatus = false;
            }
            reservationTransaction.TransactionStatus = "Failed";
            _context.Transactions.Update(reservationTransaction);

            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ResponseReservationDto>(reservationTransaction.Reservation);
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message, ex);
        }

    }

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken)
    {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x =>
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate) &&
                x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }

}
