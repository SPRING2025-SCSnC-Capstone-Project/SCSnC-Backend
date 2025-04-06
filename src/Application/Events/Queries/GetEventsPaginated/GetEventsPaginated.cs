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
        IQueryable<Event> query = new List<Event>().AsQueryable();
        
        switch(request.Filter)
        {
            case "Upcoming":
                query = _context.Events
                    .Where(x => x.EventStartDate.ToDateTimeUnspecified() >= DateTime.Now && x.IsActive == true)
                    .AsQueryable();
                break;
            case "Finished":
                 query = _context.Events
                    .Where(x => x.EventStartDate.ToDateTimeUnspecified() < DateTime.Now && x.IsActive == true)
                    .AsQueryable();
                break;
            default:
                query = _context.Events.AsQueryable();
                break;
        }
        
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