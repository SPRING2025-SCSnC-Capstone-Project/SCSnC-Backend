namespace Domain.Entities;

public class ShiftSelection : BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public LocalDate WeekStart { get; set; }
    public LocalDate Date { get; set; }
    [ForeignKey("ShiftTypeId")]
    public Guid ShiftTypeId { get; set; }
    public string Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
    public virtual Branch Branch { get; set; } = null!;
    public virtual ShiftType ShiftType { get; set; } = null!; 
}