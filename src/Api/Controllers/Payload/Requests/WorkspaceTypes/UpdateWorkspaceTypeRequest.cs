using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Api.Controllers.Payload.Requests;

public class UpdateWorkspaceTypeRequest {
    public string? WorkspaceTypeName { get; set; } = null!;
    public int? MaxCapacity { get; set; }
    public double? PricePerHour { get; set; }
    public WorkspaceUtilityServiceDto[] WorkspaceUtilityServices { get; set; }
    public IFormFile? ModelFile { get; set; }

}