namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceTypeRequest {
    public string WorkspaceTypeName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }
    public WorkspaceMediaRequest[] WorkspaceMedias { get; set; } = null!;
}

public class WorkspaceMediaRequest
{
    public string MediaType { get; set; } = null!;
    public string MediaUrl { get; set; } = null!;
}