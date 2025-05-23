namespace Domain.Entities;

public class RegistrationWindow : BaseEntity
{
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public LocalDate WeekStart { get; set; }
    public LocalDateTime OpenAt { get; set; }
    public LocalDateTime CloseAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;
}