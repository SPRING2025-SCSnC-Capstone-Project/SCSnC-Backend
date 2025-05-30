using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Diagnostics;

namespace Application.ShiftSelections.Queries.GetTotalWorkHoursOfWeek;

public record GetTotalWorkHoursOfWeekQuery : IRequest<WorkHoursDto> {
    public Guid BranchId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly WeekStart { get; set; }
}

public class GetTotalWorkHoursOfWeekQueryHandler : IRequestHandler<GetTotalWorkHoursOfWeekQuery, WorkHoursDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTotalWorkHoursOfWeekQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkHoursDto> Handle(GetTotalWorkHoursOfWeekQuery request, CancellationToken cancellationToken) {
        var employee = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.EmployeeId, cancellationToken);
        if (employee == null) {
            throw new KeyNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId, cancellationToken);
        if (branch == null) {
            throw new KeyNotFoundException($"Branch with id {request.BranchId} not found");
        }

        var shifts = await _context.AttendanceRecords.Where(s => s.BranchId == request.BranchId 
            && s.UserId == request.EmployeeId 
            && s.Date.ToDateOnly() >= request.WeekStart 
            && s.Date.ToDateOnly() <= request.WeekStart.AddDays(6)
            && (s.Status == "Present" || s.Status == "Late")).ToListAsync(cancellationToken);

        var result = new WorkHoursDto {
            Branch = _mapper.Map<BranchDto>(branch),
            Employee = _mapper.Map<UserDto>(employee),
            WeekStart = request.WeekStart,
            ShiftHours = shifts.Select(s => new ShiftHoursDto {
                Date = s.Date.ToDateOnly(),
                ShiftName = s.ShiftType.Name,
                DurationByHours = s.Status == "Present" ? (int)(s.ShiftType.EndTime - s.ShiftType.StartTime).Hours : (int)(s.ShiftType.EndTime -s.CheckInAt).Hours,
                Status = s.Status
            }).ToList()
        };
        
        return result;
    }
}
