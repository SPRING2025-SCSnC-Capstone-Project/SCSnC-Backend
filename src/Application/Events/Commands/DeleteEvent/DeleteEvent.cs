using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Ardalis.GuardClauses;

namespace Application.Events.Commands.DeleteEvent;

public record DeleteEventCommand : IRequest<EventDto>
{
    public Guid EventId { get; set; }
}

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, EventDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteEventCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventDto> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Event with id {request.EventId} not found");
        }

        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EventDto>(entity);
    }
}