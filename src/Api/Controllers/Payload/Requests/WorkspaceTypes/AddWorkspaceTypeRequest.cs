namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceTypeRequest {
    public string WorkspaceTypeName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }

}