using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Diagnostics;

namespace Application.Events.Queries.GetEventsPaginated;

public record GetNewEventsInGivenDayQuery : IRequest<PaginatedList<EventDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public int GivenDays { get; init; }
}

public class GetNewEventsInGivenDayQueryHandler : IRequestHandler<GetNewEventsInGivenDayQuery, PaginatedList<EventDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNewEventsInGivenDayQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EventDto>> Handle(GetNewEventsInGivenDayQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var givenDays = LocalDate.FromDateTime(now.AddDays(request.GivenDays));
        var events = _context.Events
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
            .Include(x => x.EventSlots)
                .ThenInclude(y => y.Slot)
            .Where(x => x.EventDate >= LocalDate.FromDateTime(now) && x.EventDate <= givenDays && x.Reservation.IsCanceled == false && x.IsCanceled == false && x.IsActive == true && x.IsPrivate == false)
            .AsQueryable();


        return await events.ListPaginateWithSortAsync<Event, EventDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}
