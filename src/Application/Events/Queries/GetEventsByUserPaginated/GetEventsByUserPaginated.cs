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
        IQueryable<Event> query = new List<Event>().AsQueryable();
        
        switch(request.Filter)
        {
            case "Upcoming":
                query = _context.Events
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceType)
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.User)
                    .Where(x => (x.Reservation.ReservationDate.ToDateOnly() > DateOnly.FromDateTime(DateTime.Now) ||
                            (x.Reservation.ReservationDate.ToDateOnly() == DateOnly.FromDateTime(DateTime.Now) &&
                            x.EventStartTime.ToTimeOnly() > TimeOnly.FromDateTime(DateTime.Now))) && 
                            x.IsActive && x.Reservation.UserId == request.UserId)
                    .AsQueryable();
                break;
            case "Finished":
                query = _context.Events
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceType)
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.User)
                    .Where(x => (x.Reservation.ReservationDate.ToDateOnly() < DateOnly.FromDateTime(DateTime.Now) ||
                                (x.Reservation.ReservationDate.ToDateOnly() == DateOnly.FromDateTime(DateTime.Now) &&
                                 x.EventEndTime.ToTimeOnly() < TimeOnly.FromDateTime(DateTime.Now))) && 
                            x.IsActive && x.Reservation.UserId == request.UserId)
                    .AsQueryable();
                break;
            default:
                query = _context.Events
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.Workspace)
                    .ThenInclude(z => z.WorkspaceType)
                    .Include(x => x.Reservation)
                    .ThenInclude(y => y.User)
                    .Where(x => x.Reservation.UserId == request.UserId)
                    .AsQueryable();
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
