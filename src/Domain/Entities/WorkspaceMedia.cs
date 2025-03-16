namespace Domain.Entities;

public class WorkspaceMedia: BaseEntity
{
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    public string MediaType { get; set; }
    public string MediaUrl { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Workspace Workspace { get; set; }
}