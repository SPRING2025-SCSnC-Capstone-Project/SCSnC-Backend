namespace Application.Common.Models.Dtos;

public class ScheduleDto{
    public BranchDto Branch { get; set; } = null!;
    public UserDto Employee { get; set; } = null!;
    public DateOnly WeekStart { get; set; }

    public List<ShiftDto> Shifts { get; set; } = [];
}

public class ShiftDto{
    public DateOnly Date { get; set; }
    public ShiftTypeDto ShiftType { get; set; } = null!;
    public string Status { get; set; } = null!;
}

