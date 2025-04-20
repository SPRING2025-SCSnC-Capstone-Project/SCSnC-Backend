using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using OneOf.Types;
using System.Diagnostics;

namespace Application.Workspaces.Queries;

public record GetWorkspacesByTimeAndTypeQuery : IRequest<List<WorkspaceDto>>
{
    public Guid[] SlotIds { get; init; } = null!;
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public Guid BranchId { get; init; }

}

public class GetWorkspacesByTimePaginatedQueryHandler : IRequestHandler<GetWorkspacesByTimeAndTypeQuery, List<WorkspaceDto>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspacesByTimePaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkspaceDto>> Handle(GetWorkspacesByTimeAndTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<WorkspaceDto> workspaceDtos = new List<WorkspaceDto>();
            var workspaces = GetRoom(request.WorkspaceTypeId, request.ReservationDate, request.SlotIds, request.BranchId).Result.AsQueryable();

            foreach (var workspace in workspaces)
            {
                WorkspaceDto workspaceDto = _mapper.Map<WorkspaceDto>(workspace);
                workspaceDtos.Add(workspaceDto);
            }

            return workspaceDtos.OrderBy(x => x.WorkspaceNumber).ToList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message);
        }

    }

    async Task<List<Workspace>> GetRoom(Guid workspaceTypeId, DateOnly reserveDate, Guid[] slotsId, Guid branchId)
    {
        var allReservationByWorkspaceTypeAndReservedDate = _context.Reservations
            .Include(x => x.ReservedSlots)
                .ThenInclude(y => y.Slot)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(w => w.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch)
                .ThenInclude(z => z.Branch)
            .AsNoTracking()
            .Include(x => x.Transactions)
            .Where(x => x.Workspace.WorkspaceTypeAtBranch.WorkspaceType.Id.Equals(workspaceTypeId)
            && x.ReserveDate.Equals(new LocalDate(reserveDate.Year, reserveDate.Month, reserveDate.Day)) 
            && x.Workspace.WorkspaceTypeAtBranch.Branch.Id.Equals(branchId))
            .AsQueryable();

        allReservationByWorkspaceTypeAndReservedDate = allReservationByWorkspaceTypeAndReservedDate.Where(x => x.Transactions.ToList()[0].TransactionStatus != "Failed");

        Debug.WriteLine(allReservationByWorkspaceTypeAndReservedDate.ToList().Count);

        var requestedTimeSlot = _context.Slots.Where(x => slotsId.Contains(x.Id)).AsQueryable();
        int[] requestedTimeRange = requestedTimeSlot.Select(x => x.SlotNumber).ToArray();
        List<Workspace> reservedWorkspaces = new List<Workspace>();
        List<Workspace> availableWorkspaces = _context.Workspaces
            .Include(x => x.WorkspaceMedias)
            .Include(x => x.WorkspaceTypeAtBranch)
                .ThenInclude(y => y.WorkspaceType)
            .Include(x => x.WorkspaceTypeAtBranch)
                .ThenInclude(y => y.Branch)
            .AsNoTracking()
            .Where(x => x.IsActive && x.WorkspaceTypeAtBranch.WorkspaceType.Id.Equals(workspaceTypeId) && x.WorkspaceTypeAtBranch.Branch.Id.Equals(branchId))
            .ToList();

        Debug.WriteLine(availableWorkspaces.Count);

        if (!allReservationByWorkspaceTypeAndReservedDate.Any())
        {
            return availableWorkspaces;
        }
        foreach (var reservation in allReservationByWorkspaceTypeAndReservedDate)
        {
            var reservedSlot = reservation.ReservedSlots.ToList();
            int[] reservedTimeRange = reservedSlot.Select(x => x.Slot.SlotNumber).ToArray();
            Array.Sort(reservedTimeRange);
            bool inRange =
                (reservedTimeRange[0] <= requestedTimeRange[0] && requestedTimeRange[0] <= reservedTimeRange[reservedTimeRange.Length - 1])
                ||
                (reservedTimeRange[0] <= requestedTimeRange[requestedTimeRange.Length - 1] && requestedTimeRange[requestedTimeRange.Length - 1] <= reservedTimeRange[reservedTimeRange.Length - 1])
                ;
            if (inRange)
            {
                reservedWorkspaces.Add(reservation.Workspace);
            }
        }
        Debug.WriteLine(reservedWorkspaces.Count);
        availableWorkspaces = availableWorkspaces.Where(x => !reservedWorkspaces.Any(y => y.WorkspaceNumber == x.WorkspaceNumber)).ToList();
        return availableWorkspaces;
    }
}