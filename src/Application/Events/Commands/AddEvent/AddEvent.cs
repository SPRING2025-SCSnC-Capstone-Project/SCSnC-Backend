using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Events.Commands;

public record AddEventCommand : IRequest<EventDto> {
    public string EventTitle { get; init; } = null!;
    public string EventDescription { get; init; } = null!;
    public string? CoverImageLink { get; init; }
    public double EntranceFee { get; init; }
    public Guid UserId { get; init; }
    public Guid ReservationId { get; init; }
    public Guid[] SlotIds { get; init; } = null!; 
}

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, EventDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddEventCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<EventDto> Handle(AddEventCommand request, CancellationToken cancellationToken) {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(x => x.Id == request.ReservationId &&
                    x.UserId == request.UserId, cancellationToken);

        if (reservation is null) {
            throw new KeyNotFoundException($"Reservation with Id {request.ReservationId} does not exist");
        }

        if (user is null) {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        foreach (var slotId in request.SlotIds) {
            var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == slotId && x.IsActive, cancellationToken);
            
            if (slot is null) {
                throw new KeyNotFoundException($"Slot with Id {slotId} does not exist");
            }

            var reservedSlot = await _context.ReservedSlots.FirstOrDefaultAsync(x => 
                x.ReservationId == request.ReservationId && 
                x.SlotId == slotId, cancellationToken);

            if (reservedSlot is null) {
                throw new KeyNotFoundException("Event slots must be within the reserved slots");
            }

            var conflict = await _context.EventSlots
                .Include(x => x.Event)
                .FirstOrDefaultAsync(x => 
                x.Event!.ReservationId == request.ReservationId && x.SlotId == slotId, cancellationToken);

            if (conflict is not null) {
                throw new ConflictException("Slot(s) has been taken for another event");
            }
        } 

        var entity = new Event() {
            EventTitle = request.EventTitle,
            EventDescription = request.EventDescription,
            EventDate = reservation.ReserveDate,
            CoverImageLink = "",
            EntranceFee = request.EntranceFee,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            ReservationId = request.ReservationId, 
            IsActive = true,
            Status = "Accepted",
        };

        var result = await _context.Events.AddAsync(entity, cancellationToken);
        
        var eventSlotsToAdd = new List<EventSlot>();

        foreach (var slotId in request.SlotIds) {
            var eventSlot = new EventSlot() {
                SlotId = slotId,
                EventId = result.Entity.Id,
            };

            eventSlotsToAdd.Add(eventSlot);
        }

        await _context.EventSlots.AddRangeAsync(eventSlotsToAdd, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var added_event = await _context.Events
            .Include(x => x.Reservation)
            .ThenInclude(y => y.Workspace)
            .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Reservation)
            .ThenInclude(y => y.User)
            .Include(x => x.EventSlots)
            .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken); 

        return _mapper.Map<EventDto>(added_event);
    }
}
