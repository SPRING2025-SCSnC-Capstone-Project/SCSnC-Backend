using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Diagnostics;

namespace Application.Events.Queries.GetEventsPaginated;

public record GetEventsPaginatedQuery : IRequest<PaginatedList<EventDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
    public bool GetAllEventByBranch { get; init; } = false;
    public Guid? BranchId { get; init; }
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
        IClock clock = SystemClock.Instance;
        DateTimeZone timeZone = DateTimeZoneProviders.Tzdb["Asia/Ho_Chi_Minh"];
        ZonedDateTime currentZonedDateTime = clock.GetCurrentInstant().InZone(timeZone);
        LocalDate currentDate = currentZonedDateTime.Date;
        LocalTime currentTime = currentZonedDateTime.TimeOfDay;
        Debug.WriteLine(currentDate);
        Debug.WriteLine(currentTime);
        IQueryable<Event> query = query = request.GetAllEventByBranch ?
                        _context.Events
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                        .ThenInclude(z => z.WorkspaceTypeAtBranch.WorkspaceType)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                        .ThenInclude(z => z.WorkspaceTypeAtBranch.Branch)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.User)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Transactions)
                    .AsNoTracking()
                    .Include(x => x.EventSlots)
                        .ThenInclude(y => y.Slot)
                    .Where(x => x.Reservation.Workspace.WorkspaceTypeAtBranch.Branch.Id.Equals(request.BranchId))
                    .OrderBy(x => x.CreatedAt)
                    .AsQueryable()
                        :
            _context.Events
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                        .ThenInclude(z => z.WorkspaceTypeAtBranch.WorkspaceType)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Workspace)
                        .ThenInclude(z => z.WorkspaceTypeAtBranch.Branch)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.User)
                    .Include(x => x.Reservation)
                        .ThenInclude(y => y.Transactions)
                    .AsNoTracking()
                    .Include(x => x.EventSlots)
                        .ThenInclude(y => y.Slot)
                    .Where(x => !x.IsPrivate
                        && x.IsActive == true
                        && x.Reservation.Transactions.ToList()[0].TransactionStatus.ToLower() == "success"
                        && x.EventDate > currentDate ? true : x.EventDate >= currentDate && x.EventSlots.ToList()[0].Slot.TimeStart >= currentTime)
                    .OrderBy(x => x.CreatedAt)
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
