using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Reservations.Commands;

public record CreateReservationCommand : IRequest<ReservationDto> {
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceId { get; init; }
    public double Deposit { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public Guid UserId { get; init; }
    public double TotalPrice { get; init; }
}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReservationCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == request.WorkspaceId && x.IsActive, cancellationToken);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null) {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        if (await CheckConflict(request, cancellationToken)) {
            throw new ConflictException($"Conflicts between reservations time range");
        }

        var entity = new Reservation() {
            UserId = request.UserId,
            WorkspaceId = request.WorkspaceId,
            ReservationDate = LocalDate.FromDateOnly(request.ReservationDate),
            StartTime = LocalTime.FromTimeOnly(request.StartTime),
            EndTime = LocalTime.FromTimeOnly(request.EndTime),
            Deposit = request.Deposit,
            IsFullPaid = false,
            TotalPrice = request.TotalPrice,
        };

        var result = await _context.Reservations.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var created_reservation = await _context.Reservations
            .Include(x => x.Workspace)
            .ThenInclude(y => y.WorkspaceType)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);

        return _mapper.Map<ReservationDto>(created_reservation);
    }

    private async Task<bool> CheckConflict(CreateReservationCommand request, CancellationToken cancellationToken) {
        var conflict = await _context.Reservations
            .Include(x => x.Workspace)
            .FirstOrDefaultAsync(x => x.Workspace.Id == request.WorkspaceId && x.ReservationDate == LocalDate.FromDateOnly(request.ReservationDate) &&
            (( x.StartTime <= LocalTime.FromTimeOnly(request.StartTime) && x.EndTime >= LocalTime.FromTimeOnly(request.StartTime) )
                || ( x.EndTime >= LocalTime.FromTimeOnly(request.EndTime) && x.StartTime <= LocalTime.FromTimeOnly(request.EndTime) ))
        , cancellationToken);

        return conflict is not null;
    }
}
