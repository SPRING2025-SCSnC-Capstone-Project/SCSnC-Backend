using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using System.Diagnostics;

namespace Application.Reservations.Queries.GetReservationsPaginated;

public record GetReservationsPaginatedQuery : IRequest<PaginatedList<ReservationDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
    public bool GetAllReservationByBranch { get; init; } = false;
    public Guid? BranchId { get; init; }
}

public class GetReservationsPaginatedQueryHandler : IRequestHandler<GetReservationsPaginatedQuery, PaginatedList<ReservationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReservationsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReservationDto>> Handle(GetReservationsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = request.GetAllReservationByBranch ?
                        _context.Reservations
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.Branch)
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .Where(x => x.Branch.Id.Equals(request.BranchId))
            .AsQueryable()
        :
        _context.Reservations
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.Branch)
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .AsQueryable();

        return await query.ListPaginateWithSortAsync<Reservation, ReservationDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}
