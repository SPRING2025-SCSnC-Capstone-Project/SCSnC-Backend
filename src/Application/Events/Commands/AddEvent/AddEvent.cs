using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Events.Commands;

public record AddEventCommand : IRequest<EventDto> {
    public string EventTitle { get; init; }
    public string EventDescription { get; init; }
    public string? CoverImageLink { get; init; }
    public double EntranceFee { get; init; }
    public DateTime EventStartDate { get; init; }
    public DateTime EventEndDate { get; init; }
    public Guid WorkspaceId { get; init; }
    public Guid UserId { get; init; }
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
        var @event = await _context.Events.FirstOrDefaultAsync(x => x.EventTitle.Equals(request.EventTitle) 
            && x.IsActive, cancellationToken);

        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == request.WorkspaceId && x.IsActive, cancellationToken);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        if (@event is not null) {
            throw new ConflictException($"Event with title {request.EventTitle} already exists");
        }

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null) {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        if (await CheckConflict(request, cancellationToken)) {
            throw new ConflictException($"Event conflicts with another registered event"); 
        }

        var event_to_add = new Event() {
            EventTitle = request.EventTitle,
            EventDescription = request.EventDescription,
            CoverImageLink = "",
            EntranceFee = request.EntranceFee,
            EventStartDate = LocalDateTime.FromDateTime(request.EventStartDate),
            EventEndDate = LocalDateTime.FromDateTime(request.EventEndDate),
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            WorkspaceId = request.WorkspaceId,
            UserId = request.UserId,
            IsActive = true,
            Status = "Accepted"
        };

        var result = await _context.Events.AddAsync(event_to_add, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventDto>(result.Entity);
    }

    private async Task<bool> CheckConflict(AddEventCommand request, CancellationToken cancellationToken) {
        var conflict = await _context.Events.Include(x => x.Workspace).FirstOrDefaultAsync(x => x.Workspace.Id == request.WorkspaceId &&
            (( x.EventStartDate <= LocalDateTime.FromDateTime(request.EventStartDate) && x.EventEndDate >= LocalDateTime.FromDateTime(request.EventStartDate) )
                || ( x.EventEndDate >= LocalDateTime.FromDateTime(request.EventEndDate) && x.EventStartDate <= LocalDateTime.FromDateTime(request.EventEndDate) ))
        , cancellationToken);

        return conflict is not null;
    }
}
