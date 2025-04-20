namespace Domain.Entities;

public class WorkspaceUtilityService : BaseEntity {
    public WorkspaceUtilityService() {
        ReservationUtilityServices = new HashSet<ReservationUtilityService>();
    }

    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    [ForeignKey("UtilityServiceId")]
    public Guid UtilityServiceId { get; set; }
    public double ServiceFee { get; set; }

    public virtual Workspace Workspace { get; set; } = null!;
    public virtual UtilityService UtilityService { get; set; } = null!;
    public virtual ICollection<ReservationUtilityService> ReservationUtilityServices { get; set; }
}
