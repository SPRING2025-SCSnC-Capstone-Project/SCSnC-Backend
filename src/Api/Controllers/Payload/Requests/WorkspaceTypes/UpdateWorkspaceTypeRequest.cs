using Application.Common.Models.Dtos;
using Application.WorkspaceTypes.Commands;
using Domain.Entities;

namespace Api.Controllers.Payload.Requests;

public class UpdateWorkspaceTypeRequest {
    public string? WorkspaceTypeName { get; set; } = null!;
    public string? WorkspaceTypeDescription { get; set; } = null!;
    public int? MaxCapacity { get; set; }
    public double? PricePerHour { get; set; }
    public string? WorkspaceUtilityServices { get; set; }
    public string? WorkspacesAtBranches { get; set; }
    public string? WorkspaceInWorkspaceTypes { get; set; }
    public string? UpdateWorkspaceTypeImages { get; set; }
    public IFormFile[]? NewImages { get; set; }
    public bool IsActive { get; set; }
    public IFormFile? ModelFile { get; set; }

}