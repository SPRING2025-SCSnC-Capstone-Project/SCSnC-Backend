using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Events.Queries.GetEventById;

public record GetEventByIdQuery: IRequest<EventDto>
{
    public Guid Id { get; set; }
}

public class GetEventByIdQueryHandler: IRequestHandler<GetEventByIdQuery, EventDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetEventByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = _context.Events
            .Include(x => x.Reservation)
                .ThenInclude(y => y.Workspace)
                .ThenInclude(z => z.WorkspaceTypeAtBranch)
                .ThenInclude(w => w.WorkspaceType)
            .Include(x => x.Reservation)
                .ThenInclude(y => y.Workspace)
                .ThenInclude(z => z.WorkspaceTypeAtBranch)
                .ThenInclude(w => w.Branch)
            .AsNoTracking()
            .Include(x => x.Reservation)
                .ThenInclude(y => y.User)   
            .Include(x => x.EventSlots)
                .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Event with id {request.Id} not found");
        }
        
        var result = _mapper.Map<EventDto>(await entity);

        return result;
    }
}
