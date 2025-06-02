using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Events.Queries.GetEventsByUserPaginated;

public record GetEventsByUserPaginatedQuery : IRequest<PaginatedList<EventDto>>
{
    public Guid UserId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
}

public class GetEventsByUserPaginatedQueryHandler : IRequestHandler<GetEventsByUserPaginatedQuery, PaginatedList<EventDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetEventsByUserPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<EventDto>> Handle(GetEventsByUserPaginatedQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Event> query = 
                query = _context.Events
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                            .ThenInclude(z => z.WorkspaceTypeAtBranch.WorkspaceType)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                            .ThenInclude(z => z.WorkspaceTypeAtBranch.Branch)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.User)
                    .Include(x => x.EventSlots)
                        .ThenInclude(y => y.Slot)
                    .Where(x => x.Reservation.UserId == request.UserId)
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
