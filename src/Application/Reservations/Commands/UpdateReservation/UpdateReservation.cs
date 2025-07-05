using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Reservations;

public record UpdateReservationCommand : IRequest<ReservationDto> {
    public Guid Id { get; init; }
    public string? Status { get; init; }
    public bool? IsFullPaid { get; init; }
}

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateReservationCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReservationDto> Handle(UpdateReservationCommand request, CancellationToken cancellationToken) {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == request.Id
            && (x.Status.Equals("Pending") || x.Status.Equals("Booked")), cancellationToken);

        if (reservation is null) {
            throw new KeyNotFoundException($"Reservation with Id {request.Id} does not exist");
        }

        reservation.Status = request.Status ?? reservation.Status;
        reservation.IsFullPaid = request.IsFullPaid ?? reservation.IsFullPaid;

        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReservationDto>(reservation);
    }
}
