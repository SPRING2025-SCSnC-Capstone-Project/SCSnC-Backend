using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Application.Reservations.Queries.GetReservationsByUserPaginated;

public record GetReservationsByUserPaginatedQuery : IRequest<PaginatedList<ReservationDto>>
{
    public Guid UserId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
}

public class GetReservationsByUserPaginatedQueryHandler : IRequestHandler<GetReservationsByUserPaginatedQuery, PaginatedList<ReservationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReservationsByUserPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReservationDto>> Handle(GetReservationsByUserPaginatedQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == request.UserId);
        var query = 
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
            .Where(user != null? x => x.UserId == request.UserId || x.Phone.Equals(user.Phone) || x.Email.Equals(user.Email) : x => x.UserId == request.UserId)
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
