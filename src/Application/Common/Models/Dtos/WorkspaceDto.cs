using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceDto : BaseDto, IMapFrom<Workspace> {
    public int WorkspaceNumber { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public bool IsAvailable { get; set; }
    public WorkspaceTypeDto WorkspaceType { get; set; } = null!;
    public WorkspaceMediaDto[] WorkspaceMedias { get; set; } = null!;
}
