using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceDto : BaseDto, IMapFrom<Workspace> {
    public Guid WorkspaceTypeId { get; set; }
    public bool IsAvailable { get; set; }
    public string? WorkspaceImageUrl { get; set; }
}