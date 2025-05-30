using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class AttendanceRecordDto {
    public DateOnly Date { get; set; }
    public TimeOnly CheckInAt { get; set; }
    public string Status { get; set; } = null!;

    public UserDto User { get; set; } = null!;
    public BranchDto Branch { get; set; } = null!;
    public ShiftTypeDto ShiftType { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttendanceRecord, AttendanceRecordDto>()
            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date.ToDateOnly()))
            .ForMember(d => d.CheckInAt, opt => opt.MapFrom(s => s.CheckInAt.ToTimeOnly()));
    }
}
