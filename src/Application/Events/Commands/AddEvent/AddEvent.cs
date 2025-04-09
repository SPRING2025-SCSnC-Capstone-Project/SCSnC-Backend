using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Events.Commands;

public record AddEventCommand : IRequest<EventDto>
{
    public string EventTitle { get; init; } = null!;
    public string EventDescription { get; init; } = null!;
    public string? CoverImageLink { get; init; }
    public double EntranceFee { get; init; }
    public TimeOnly EventStartTime { get; init; }
    public TimeOnly EventEndTime { get; init; }
    public Guid ReservationId { get; init; }
    public Guid UserId { get; init; }
}

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, EventDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddEventCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<EventDto> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(x => x.Id == request.ReservationId &&
                    x.UserId == request.UserId, cancellationToken);

        if (reservation is null)
        {
            throw new KeyNotFoundException($"Reservation with Id {request.ReservationId} does not exist");
        }

        if (user is null)
        {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        if (reservation.StartTime > LocalTime.FromTimeOnly(request.EventStartTime) || reservation.EndTime < LocalTime.FromTimeOnly(request.EventEndTime)
                || reservation.EndTime < LocalTime.FromTimeOnly(request.EventStartTime) || reservation.StartTime > LocalTime.FromTimeOnly(request.EventEndTime))
        {
            throw new ConflictException($"Event time range must be registered within reserved time range");
        }

        var entity = new Event()
        {
            EventTitle = request.EventTitle,
            EventDescription = request.EventDescription,
            CoverImageLink = "",
            EntranceFee = request.EntranceFee,
            EventStartTime = LocalTime.FromTimeOnly(request.EventStartTime),
            EventEndTime = LocalTime.FromTimeOnly(request.EventEndTime),
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            ReservationId = request.ReservationId,
            IsActive = true,
            Status = "Accepted"
        };

        var result = await _context.Events.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var added_event = _context.Events
            .Include(x => x.Reservation)
            .ThenInclude(y => y.Workspace)
            .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Reservation)
            .ThenInclude(y => y.User)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);

        return _mapper.Map<EventDto>(added_event);
    }
}