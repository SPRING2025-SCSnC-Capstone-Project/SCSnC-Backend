namespace Api.Controllers.Payload.Requests;

public class UpdateWorkspaceTypeRequest {
    public string WorkspaceTypeName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }

}