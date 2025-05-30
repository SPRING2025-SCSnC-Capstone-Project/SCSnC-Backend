using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using NodaTime.Extensions;

namespace Application.AttendanceRecords.Commands.CheckIn;

public record CheckInCommand : IRequest<AttendanceRecordDto> {
    public Guid BranchId { get; set; }
    public Guid ShiftTypeId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly CheckInDate { get; set; }
}

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, AttendanceRecordDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CheckInCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AttendanceRecordDto> Handle(CheckInCommand request, CancellationToken cancellationToken) {
        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId, cancellationToken);

        if (branch is null) {
            throw new KeyNotFoundException($"Branch with id {request.BranchId} not found");
        }

        var shiftType = await _context.ShiftTypes.FirstOrDefaultAsync(s => s.Id == request.ShiftTypeId, cancellationToken);

        if (shiftType is null) {
            throw new KeyNotFoundException($"Shift type with id {request.ShiftTypeId} not found");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.EmployeeId, cancellationToken);

        if (user is null) {
            throw new KeyNotFoundException($"User with id {request.EmployeeId} not found");
        }

        if (shiftType.BranchId != branch.Id) {
            throw new ConflictException($"Shift type with id {request.ShiftTypeId} does not belong to branch with id {request.BranchId}");
        }

        var attendanceRecord = new AttendanceRecord() {
            BranchId = branch.Id,
            ShiftTypeId = shiftType.Id,
            UserId = user.Id,
            Date = request.CheckInDate.ToLocalDate(),
            CheckInAt = TimeOnly.FromDateTime(DateTime.Now).ToLocalTime(),
        };

        if (TimeOnly.FromDateTime(DateTime.Now) - shiftType.StartTime.ToTimeOnly() >= TimeSpan.FromMinutes(15)) {
            attendanceRecord.Status = "Late";
        } else {
            attendanceRecord.Status = "Present";
        }

        await _context.AttendanceRecords.AddAsync(attendanceRecord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttendanceRecordDto>(attendanceRecord);
    }
}
