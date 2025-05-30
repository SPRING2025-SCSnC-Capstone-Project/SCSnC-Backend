using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using MediatR;
using NodaTime;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Application.Reservations.Queries.GetReservationsByUserPaginated;

public record GetReservationsByUserPaginatedQuery : IRequest<PaginatedList<ReservationDto>>
{
    public Guid UserId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Filter { get; init; }
    public bool GetLatestReservation { get; init; }
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
        try
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == request.UserId);
            var now = DateTime.UtcNow;
            var todayDateTime = new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            List<Reservation> latestReservationList = new List<Reservation>();
            List<Reservation> currentlyInPendingReservationList = new List<Reservation>();
            var query =
                _context.Reservations
                .Include(x => x.Workspace)
                    .ThenInclude(y => y.WorkspaceTypeAtBranch)
                    .ThenInclude(z => z.WorkspaceType)
                .Include(x => x.Workspace)
                    .ThenInclude(y => y.WorkspaceTypeAtBranch)
                    .ThenInclude(z => z.Branch)
                .Include(x => x.User)
                .Include(x => x.ReservedSlots)
                    .ThenInclude(y => y.Slot)
                .Where(user != null ? x => x.UserId == request.UserId || x.Phone.Equals(user.Phone) || x.Email.Equals(user.Email) : x => x.UserId == request.UserId)
                .AsQueryable();

            await AutoUpdateReservationStatus(query, cancellationToken);

            if (request.GetLatestReservation == true)
            {
                foreach (var reservation in query)
                {
                    if (reservation.IsCanceled == false && reservation.Status.ToLower() == "booked")
                    {
                        if (reservation.ReservedSlots != null && reservation.ReservedSlots.Count > 0)
                        {
                            var reservedDate = reservation.ReserveDate;
                            var reservedStartTime = reservation.ReservedSlots.ToList()[0].Slot.TimeStart;
                            var reservationDateTime = new LocalDateTime(reservedDate.Year, reservedDate.Month, reservedDate.Day, reservedStartTime.Hour, reservedStartTime.Minute, reservedStartTime.Second);
                            if (todayDateTime <= reservationDateTime)
                            {
                                latestReservationList.Add(reservation);
                            }
                        }
                        else
                        {
                            var reservedDate = reservation.ReserveDate;
                            var reservedStartTime = reservation.TimeStart;
                            var reservationDateTime = new LocalDateTime(reservedDate.Year, reservedDate.Month, reservedDate.Day, reservedStartTime.Value.Hour, reservedStartTime.Value.Minute, reservedStartTime.Value.Second);
                            if (todayDateTime <= reservationDateTime)
                            {
                                latestReservationList.Add(reservation);
                            }
                        }
                    }
                }
                query = _context.Reservations
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
                    .Where(user != null ? x => x.UserId == request.UserId || x.Phone.Equals(user.Phone) || x.Email.Equals(user.Email) : x => x.UserId == request.UserId)
                    .Where(x => latestReservationList.Contains(x))
                    .AsQueryable();
            }
            else
            {
                query = _context.Reservations
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
                    .Where(user != null ? x => x.UserId == request.UserId || x.Phone.Equals(user.Phone) || x.Email.Equals(user.Email) : x => x.UserId == request.UserId)
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
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IQueryable<Reservation>> AutoUpdateReservationStatus (IQueryable<Reservation> query, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var todayDateTime = new LocalDateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
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
                        if (todayDateTime > reservationDateTime)
                        {
                            Debug.WriteLine(reservation.Id);
                            reservation.Status = "Done";
                            _context.Reservations.Update(reservation);
                            updateIndicator++;
                        }
                    }
                    else
                    {
                        var reservedDate = reservation.ReserveDate;
                        var reservedEndTime = reservation.TimeStart;
                        var reservationDateTime = new LocalDateTime(reservedDate.Year, reservedDate.Month, reservedDate.Day, reservedEndTime.Value.Hour, reservedEndTime.Value.Minute, reservedEndTime.Value.Second);
                        if (todayDateTime > reservationDateTime)
                        {
                            Debug.WriteLine(reservation.Id);
                            reservation.Status = "Done";
                            _context.Reservations.Update(reservation);
                            updateIndicator++;
                        }
                    }
                    break;
            }
        }

        if (updateIndicator > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        return query;
    }
}
