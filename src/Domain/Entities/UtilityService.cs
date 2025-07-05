namespace Domain.Entities;

public class UtilityService : BaseEntity {
    public UtilityService() {
        WorkspaceUtilityServices = new HashSet<WorkspaceUtilityService>();
    }

    public string ServiceName { get; set; } = null!;
    public string ServiceImage { get; set; } = null!;

    public virtual ICollection<WorkspaceUtilityService> WorkspaceUtilityServices { get; set; }
}
