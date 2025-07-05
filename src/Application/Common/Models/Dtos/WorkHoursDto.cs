namespace Application.Common.Models.Dtos;

public class WorkHoursDto {
    public BranchDto Branch { get; set; } = null!;
    public UserDto Employee { get; set; } = null!;
    public DateOnly WeekStart { get; set; }

    public List<ShiftHoursDto> ShiftHours { get; set; } = new();
}

public class ShiftHoursDto {
    public DateOnly Date { get; set; }
    public string ShiftName { get; set; } = null!;
    public int DurationByHours { get; set; }
    public string Status { get; set; } = null!;
}