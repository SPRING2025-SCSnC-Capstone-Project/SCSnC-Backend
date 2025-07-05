using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ShiftSelections.Queries.GetShiftsByEmployeeAndWeekStart; 
public record GetShiftsByEmployeeAndWeekStartQuery : IRequest<ScheduleDto> {
    public Guid EmployeeId { get; init; }
    public DateOnly WeekStart { get; init; }
    public Guid BranchId { get; init; }
}

public class GetShiftsByEmployeeAndWeekStartQueryHandler : IRequestHandler<GetShiftsByEmployeeAndWeekStartQuery, ScheduleDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShiftsByEmployeeAndWeekStartQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ScheduleDto> Handle(GetShiftsByEmployeeAndWeekStartQuery request, CancellationToken cancellationToken) {
        var shifts = await _context.ShiftSelections.Where(s => s.UserId == request.EmployeeId 
            && s.WeekStart.ToDateOnly() == request.WeekStart
            && s.BranchId == request.BranchId).ToListAsync(cancellationToken);

        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId, cancellationToken);

        if (branch is null) {
            throw new KeyNotFoundException($"Branch with id {request.BranchId} not found");
        }

        var employee = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.EmployeeId, cancellationToken);

        if (employee is null) {
            throw new KeyNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        var attendanceRecords = await _context.AttendanceRecords.Where(a => a.UserId == request.EmployeeId 
            && a.Date.ToDateOnly() >= request.WeekStart
            && a.Date.ToDateOnly() <= request.WeekStart.AddDays(6)
            && a.BranchId == request.BranchId).ToListAsync(cancellationToken);

        var result = new ScheduleDto() {
            Branch = _mapper.Map<BranchDto>(branch),
            Employee = _mapper.Map<UserDto>(employee),
            WeekStart = request.WeekStart,
            Shifts = shifts.Select(s => new ShiftDto() {
                Date = s.Date.ToDateOnly(),
                ShiftType = _mapper.Map<ShiftTypeDto>(s.ShiftType),
                Status = attendanceRecords.FirstOrDefault(a => a.Date == s.Date)?.Status ?? "Selected"
            }).ToList()
        };

        return result;
    }
}


