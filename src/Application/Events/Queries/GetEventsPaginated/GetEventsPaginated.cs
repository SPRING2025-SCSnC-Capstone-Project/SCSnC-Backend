using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Events.Queries.GetEventsPaginated;

public record GetEventsPaginatedQuery : IRequest<PaginatedList<EventDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
}

public class GetEventsPaginatedQueryHandler : IRequestHandler<GetEventsPaginatedQuery, PaginatedList<EventDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetEventsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<EventDto>> Handle(GetEventsPaginatedQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Event> query = query = _context.Events
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceType)
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.User)
                    .Include(x => x.EventSlots)
                    .ThenInclude(y => y.Slot)
                    .AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Event, EventDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}
