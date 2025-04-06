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
            .FirstOrDefaultAsync(x => x.IsActive == true && x.Id == request.Id, cancellationToken);
        
        if (entity is null)
        {
            throw new KeyNotFoundException($"Event with id {request.Id} not found");
        }
        
        var result = _mapper.Map<EventDto>(await entity);

        return result;
    }
}