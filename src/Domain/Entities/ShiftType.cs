namespace Domain.Entities;

public class ShiftType : BaseEntity
{
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public string Name { get; set; } = null!;
    public LocalTime StartTime { get; set; }
    public LocalTime EndTime { get; set; }
    public bool IsActive { get; set; }

    public virtual Branch Branch { get; set; } = null!;
}