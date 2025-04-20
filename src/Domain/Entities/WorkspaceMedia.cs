namespace Domain.Entities;

public class WorkspaceMedia: BaseEntity
{
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    public string MediaType { get; set; } = null!;
    public string MediaUrl { get; set; } = null!;
    
    public virtual Workspace Workspace { get; set; }
}
