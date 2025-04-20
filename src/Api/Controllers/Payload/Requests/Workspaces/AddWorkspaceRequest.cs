namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceRequest {
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public string WorkspaceName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid BranchId { get; set; }
    public WorkspaceMediaRequest[] WorkspaceMedias { get; set; } = null!;
}

public class WorkspaceMediaRequest {
    public string MediaType { get; set; } = null!;
    public string MediaUrl { get; set; } = null!;
}
