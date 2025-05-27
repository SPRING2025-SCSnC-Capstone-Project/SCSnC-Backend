using Application.WorkspaceTypes.Commands;

namespace Api.Controllers.Payload.Requests;

public class AddWorkspaceTypeRequest {
    public string WorkspaceTypeName { get; set; } = null!;
    public string WorkspaceTypeDescription { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }
    public Guid[] BranchId { get; set; }
    public string WorkspaceInWorkspaceTypes { get; set; }
    public IFormFile[]? Images { get; set; }
}

public class WorkspaceMediaRequest
{
    public string MediaType { get; set; } = null!;
    public string MediaUrl { get; set; } = null!;
}