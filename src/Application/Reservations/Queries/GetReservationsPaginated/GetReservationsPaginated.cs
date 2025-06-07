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
        try
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
.Include(x => x.Transactions)
    .ThenInclude(y => y.Order)
        .ThenInclude(z => z.OrderDetails)
            .ThenInclude(w => w.ItemWithSize)
                .ThenInclude(r => r.Item).AsNoTracking()
.Include(x => x.Transactions)
    .ThenInclude(y => y.Order)
        .ThenInclude(z => z.OrderDetails)
            .ThenInclude(w => w.ItemWithSize)
                .ThenInclude(r => r.Size).AsNoTracking()
.Include(x => x.Transactions)
        .ThenInclude(y => y.Order)
            .ThenInclude(z => z.OrderDetails)
                .ThenInclude(w => w.IncludeToppings)
                    .ThenInclude(r => r.Topping).AsNoTracking()
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
.Include(x => x.Transactions)
    .ThenInclude(y => y.Order)
        .ThenInclude(z => z.OrderDetails)
            .ThenInclude(w => w.ItemWithSize)
                .ThenInclude(r => r.Item).AsNoTracking()
.Include(x => x.Transactions)
    .ThenInclude(y => y.Order)
        .ThenInclude(z => z.OrderDetails)
            .ThenInclude(w => w.ItemWithSize)
                .ThenInclude(r => r.Size).AsNoTracking()
.Include(x => x.Transactions)
    .ThenInclude(y => y.Order)
        .ThenInclude(z => z.OrderDetails)
            .ThenInclude(w => w.IncludeToppings)
                .ThenInclude(r => r.Topping).AsNoTracking()
    .Include(x => x.User)
    .Include(x => x.ReservedSlots)
    .ThenInclude(y => y.Slot)
    .AsQueryable();

            await AutoUpdateReservationStatus(cancellationToken);

            return await query.ListPaginateWithSortAsync<Reservation, ReservationDto>(
                request.Page,
                request.Size,
                request.SortBy,
                request.SortOrder,
                _mapper.ConfigurationProvider,
                cancellationToken
            );
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw new Exception(e.Message, e);
        }

    }
    public async Task<IQueryable<Reservation>> AutoUpdateReservationStatus(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var query = _context.Reservations.Include(x => x.ReservedSlots)
    .ThenInclude(y => y.Slot).AsQueryable();
        var instant = Instant.FromDateTimeUtc(now);
        var timeZone = DateTimeZoneProviders.Tzdb["Asia/Ho_Chi_Minh"];
        var zonedDateTime = instant.InZone(timeZone);
        var localDateTime = zonedDateTime.LocalDateTime;
        List<Reservation> latestReservationList = new List<Reservation>();
        int updateIndicator = 0;

        foreach (var reservation in query)
        {
            switch (reservation.Status.ToLower())
            {
                case "booked":
                    if (reservation.ReservedSlots != null && reservation.ReservedSlots.Count > 0)
                    {
                        var reservedDate = reservation.ReserveDate;
                        var reservedEndTime = reservation.ReservedSlots.ToList()[reservation.ReservedSlots.ToList().Count - 1].Slot.TimeEnd;
                        var reservationDateTime = new LocalDateTime(reservedDate.Year, reservedDate.Month, reservedDate.Day, reservedEndTime.Hour, reservedEndTime.Minute, reservedEndTime.Second);
                        if (localDateTime > reservationDateTime)
                        {
                            Debug.WriteLine(reservation.Id);
                            reservation.Status = "Done";
                            latestReservationList.Add(reservation);
                            updateIndicator++;
                        }
                    }
                    else
                    {
                        var reservedDate = reservation.ReserveDate;
                        var reservedEndTime = reservation.TimeStart;
                        var reservationDateTime = new LocalDateTime(reservedDate.Year, reservedDate.Month, reservedDate.Day, reservedEndTime.Value.Hour, reservedEndTime.Value.Minute, reservedEndTime.Value.Second);
                        if (localDateTime > reservationDateTime)
                        {
                            Debug.WriteLine(reservation.Id);
                            reservation.Status = "Done";
                            latestReservationList.Add(reservation);
                            updateIndicator++;
                        }
                    }
                    break;
            }
        }

        if (updateIndicator > 0)
        {
            _context.Reservations.UpdateRange(latestReservationList);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return query;
    }

}
