namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceRequest {
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public Guid WorkspaceTypeAtBranchId { get; init; }

}
