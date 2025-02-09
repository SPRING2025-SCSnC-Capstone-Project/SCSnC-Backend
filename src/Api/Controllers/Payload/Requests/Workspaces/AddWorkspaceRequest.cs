namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceRequest {
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public string? WorkspaceImageUrl { get; set; }
}