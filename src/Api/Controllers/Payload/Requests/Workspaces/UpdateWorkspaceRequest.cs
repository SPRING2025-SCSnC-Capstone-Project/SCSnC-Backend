namespace Api.Controllers.Payload.Requests;

public class UpdateWorkspaceRequest {
    public string? WorkspaceImageUrl { get; set; }
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public Guid BranchId { get; set; }
    public string WorkspaceName { get; set; } = null!;
    public string Description { get; set; } = null!;
}
