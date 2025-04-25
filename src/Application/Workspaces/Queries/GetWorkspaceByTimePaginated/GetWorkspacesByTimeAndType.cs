using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using MediatR;
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
    public bool BookingWithTime { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
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
            var workspaces = GetRoom(request.WorkspaceTypeId, request.ReservationDate, request.SlotIds, request.BranchId, request.BookingWithTime, request.TimeStart, request.TimeEnd).Result.AsQueryable();

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

    async Task<List<Workspace>> GetRoom(Guid workspaceTypeId, DateOnly reserveDate, Guid[] slotsId, Guid branchId, bool BookingWithTime, TimeOnly timeStart, TimeOnly timeEnd)
    {
        try
        {
            var requestedTimeStart = LocalTime.FromTimeOnly(timeStart);
            var requestedTimeEnd = LocalTime.FromTimeOnly(timeEnd);
            Debug.WriteLine(requestedTimeStart);
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
                var reservedSlot = reservation.ReservedSlots.OrderBy(x => x.Slot.SlotNumber).ToList();
                int[] reservedTimeRange = reservedSlot.Select(x => x.Slot.SlotNumber).ToArray();
                Array.Sort(reservedTimeRange);
                bool inRange = false;
                if (!BookingWithTime)
                {
                    if (reservedTimeRange.Length > 0)
                    {
                        inRange =
                            (reservedTimeRange[0] <= requestedTimeRange[0] && requestedTimeRange[0] <= reservedTimeRange[reservedTimeRange.Length - 1])
                            ||
                            (reservedTimeRange[0] <= requestedTimeRange[requestedTimeRange.Length - 1] && requestedTimeRange[requestedTimeRange.Length - 1] <= reservedTimeRange[reservedTimeRange.Length - 1])
                            ;
                    }

                    if (!reservation.TimeStart.Equals(LocalTime.Midnight))
                    {
                        var slots = _context.Slots.Where(x => slotsId.Any(y => x.Id.Equals(y))).OrderBy(x => x.SlotNumber).ToList();
                        var reservedTimeStart = reservation.TimeStart;
                        var reservedTimeEnd = reservation.TimeEnd;
                        var requestedSlotTimeStart = slots[0].TimeStart;
                        var requestedSlotTimeEnd = slots[slots.Count - 1].TimeEnd;

                        inRange =
                            (reservedTimeStart <= requestedSlotTimeStart && requestedSlotTimeStart <= reservedTimeEnd)
                            ||
                            (reservedTimeStart <= requestedSlotTimeEnd && requestedSlotTimeEnd <= reservedTimeEnd)
                            ;
                    }

                }
                else
                {
                    requestedTimeStart = LocalTime.FromTimeOnly(timeStart);
                    requestedTimeEnd = LocalTime.FromTimeOnly(timeEnd);

                    Debug.WriteLine("------------------------------REQUESTED--------------------------------");
                    Debug.WriteLine(requestedTimeStart);
                    Debug.WriteLine(requestedTimeEnd);

                    if (reservedTimeRange.Length > 0)
                    {
                        var reservedSlotTimeStart = reservedSlot.ToList()[0].Slot.TimeStart;
                        var reservedSlotTimeEnd = reservedSlot.ToList()[reservedSlot.ToList().Count - 1].Slot.TimeEnd;
                        Debug.WriteLine("---------------------------RESERVED SLOT TIME----------------------------------");
                        Debug.WriteLine(reservedSlotTimeStart);
                        Debug.WriteLine(reservedSlotTimeEnd);
                        inRange =
                            (reservedSlotTimeStart <= requestedTimeStart && requestedTimeStart <= reservedSlotTimeEnd)
                            ||
                            (reservedSlotTimeStart <= requestedTimeEnd && requestedTimeEnd <= reservedSlotTimeEnd)
                            ;
                    }

                    if (!reservation.TimeStart.Equals(LocalTime.Midnight))
                    {
                        var reservedTimeStart = reservation.TimeStart;
                        var reservedTimeEnd = reservation.TimeEnd;
                        Debug.WriteLine("---------------------------RESERVED TIME----------------------------------");
                        Debug.WriteLine(reservedTimeStart);
                        Debug.WriteLine(reservedTimeEnd);
                        inRange =
                            (reservedTimeStart <= requestedTimeStart && requestedTimeStart <= reservedTimeEnd)
                            ||
                            (reservedTimeStart <= requestedTimeEnd && requestedTimeEnd <= reservedTimeEnd)
                            ;
                    }
                }
                if (inRange)
                {
                    reservedWorkspaces.Add(reservation.Workspace);
                }
            }
            Debug.WriteLine(reservedWorkspaces.Count);
            availableWorkspaces = availableWorkspaces.Where(x => !reservedWorkspaces.Any(y => y.WorkspaceNumber == x.WorkspaceNumber)).ToList();
            return availableWorkspaces;
        }catch(Exception e)
        {
            Debug.WriteLine(e.Message);
            throw new Exception(e.Message, e);
        }
    }
}