namespace Domain.Entities;

public class ReservationUtilityService : BaseEntity {
    [ForeignKey("ReservationId")]
    public Guid ReservationId { get; set; }
    [ForeignKey("WorkspaceUtilityServiceId")]
    public Guid WorkspaceUtilityServiceId { get; set; }

    public virtual Reservation Reservation { get; set; } = null!;
    public virtual WorkspaceUtilityService WorkspaceUtilityService { get; set; } = null!;
}
