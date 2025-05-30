namespace Domain.Entities;

public class AttendanceRecord : BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public LocalDate Date { get; set; }
    [ForeignKey("ShiftTypeId")]
    public Guid ShiftTypeId { get; set; }
    public LocalTime CheckInAt { get; set; }
    public string Status { get; set; } = null!;

    public virtual User User { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual ShiftType ShiftType { get; set; }
}