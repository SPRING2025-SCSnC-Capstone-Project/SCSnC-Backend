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
    public bool? InFuture { get; init; } = false;
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
        var query = _context.Reservations
            .Include (x => x.Workspace)
                .ThenInclude (y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.Branch)
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .AsQueryable();

        if (request.InFuture.HasValue && request.InFuture.Value)
        {
            var date = DateTime.Now;
            var today = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
            LocalDate localDate = new LocalDate(today.Year, today.Month, today.Day);
            LocalTime localTime = new LocalTime(today.Hour, today.Minute, 0);
            Debug.WriteLine(localDate);
            Debug.WriteLine(localTime);
            query = query.Include(x => x.Transactions).Where(x => x.Transactions.ToList()[0].TransactionStatus != "Failed"
            && x.ReserveDate > localDate ? true : x.ReserveDate >= localDate && x.ReservedSlots.ToList()[0].Slot.TimeStart >= localTime)
            .AsQueryable();
        }

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
