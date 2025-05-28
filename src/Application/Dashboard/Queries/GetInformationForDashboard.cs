using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using System.Diagnostics;

namespace Application.Dashboard.Queries;

public record GetInformationForDashboardQuery : IRequest<DashboardDto>
{
    public DateOnly? GivenMonth { get; set; }
}

public class GetEventsByUserPaginatedQueryHandler : IRequestHandler<GetInformationForDashboardQuery, DashboardDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEventsByUserPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DashboardDto> Handle(GetInformationForDashboardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            double revenueInGivenMonth = 0;
            double revenueInLast7Days = 0;
            Dictionary<Guid, int> numberOfBookingForEachWorkspacetype = new Dictionary<Guid, int>();
            var now = DateTime.UtcNow;
            var nowMinus7 = now.AddDays(-7);
            var startOfMonth = new LocalDate(
                request.GivenMonth.HasValue ? request.GivenMonth.Value.Year : now.Year,
                request.GivenMonth.HasValue ? request.GivenMonth.Value.Month : now.Month,
                1
            );
            var last7Days = new LocalDate(
                nowMinus7.Year,
                nowMinus7.Month,
                nowMinus7.Day
            );
            var today = new LocalDate(
                now.Year,
                now.Month,
                now.Day
            );
            var startOfNextMonth = startOfMonth.PlusMonths(1);
            var workspaceTypes = _context.WorkspaceTypes.Where(x => x.IsActive == true).AsQueryable();
            var reservations = _context.Reservations
                .Include(x => x.Workspace)
                    .ThenInclude(y => y.WorkspaceTypeAtBranch)
                        .ThenInclude(z => z.WorkspaceType)
                .Include(x => x.Workspace)
                    .ThenInclude(y => y.Orders)
                .Where(x => x.IsCanceled == false).AsQueryable();
            var orders = _context.Orders.Include(x => x.Workspace).ThenInclude(y => y.WorkspaceTypeAtBranch).Where(x => x.IsActive == true).AsQueryable();
            var reservationsInGivenMonth = reservations.Where(x => x.ReserveDate >= startOfMonth && x.ReserveDate < startOfNextMonth);
            var reservationsInLast7Days = reservations.Where(x => x.ReserveDate <= today && x.ReserveDate >= last7Days);

            foreach (var workspaceType in workspaceTypes)
            {
                numberOfBookingForEachWorkspacetype[workspaceType.Id] = 0;
            }

            foreach (var reservation in reservationsInGivenMonth)
            {
                if (reservation.IsFullPaid == true)
                {
                    revenueInGivenMonth += reservation.TotalPrice;
                }

                if (reservation.IsFullPaid == false)
                {
                    revenueInGivenMonth += reservation.Deposit;
                }
            }

            foreach (var reservation in reservationsInGivenMonth)
            {
                if (reservation.Workspace.Orders.Count > 0)
                {
                    var ordersOfReservation = reservation.Workspace.Orders;
                    foreach (var order in ordersOfReservation)
                    {
                        var orderDateTime = new LocalDate(
                            order.CreatedAt.Year,
                            order.CreatedAt.Month,
                            order.CreatedAt.Day
                        );

                        if (orderDateTime >= startOfMonth && orderDateTime < startOfNextMonth)
                        {
                            revenueInGivenMonth += order.TotalPrice;
                        }
                    }
                }
            }

            foreach (var reservation in reservationsInLast7Days)
            {
                if (reservation.IsFullPaid == true)
                {
                    revenueInLast7Days += reservation.TotalPrice;
                }

                if (reservation.IsFullPaid == false)
                {
                    revenueInLast7Days += reservation.Deposit;
                }
            }

            foreach (var reservation in reservationsInGivenMonth)
            {
                Guid workspaceTypeId = reservation.Workspace.WorkspaceTypeAtBranch.WorkspaceTypeId;
                if (numberOfBookingForEachWorkspacetype.TryGetValue(workspaceTypeId, out int currentBookingNumber))
                {
                    numberOfBookingForEachWorkspacetype[workspaceTypeId] = currentBookingNumber + 1;
                }
            }

            foreach(var key in numberOfBookingForEachWorkspacetype)
            {
                Debug.WriteLine(key.Key);
                Debug.WriteLine(key.Value);
            }

            var filterOutWorkspaceTypeWithZeroBooking = numberOfBookingForEachWorkspacetype.Where(x => x.Value > 0);
            KeyValuePair<Guid, int>? mostBookedWorkspaceType = null;
            if (filterOutWorkspaceTypeWithZeroBooking.Any())
            {
                mostBookedWorkspaceType = numberOfBookingForEachWorkspacetype.MaxBy(x => x.Value);
            }

            var result = new DashboardDto()
            {
                MostBookedWorkspaceType = mostBookedWorkspaceType.HasValue ? _mapper.Map<WorkspaceTypeDto>(workspaceTypes.FirstOrDefault(x => x.Id.Equals(mostBookedWorkspaceType.Value.Key))) : null,
                Revenue7Days = revenueInLast7Days,
                RevenueInGivenMonth = revenueInGivenMonth
            };

            return result;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message, ex);
        }
    }
}


