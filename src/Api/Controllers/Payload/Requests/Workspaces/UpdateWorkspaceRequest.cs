namespace Api.Controllers.Payload.Requests;

public class UpdateWorkspaceRequest {
    public string? WorkspaceImageUrl { get; set; }
    public string Name { get; set; }
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
}