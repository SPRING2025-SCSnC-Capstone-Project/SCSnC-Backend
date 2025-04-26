using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceDto : BaseDto, IMapFrom<Workspace> {
    public int WorkspaceNumber { get; set; }
    public string WorkspaceName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsAvailable { get; set; }
    public BranchDto Branch { get; set; } = null!;
    public WorkspaceTypeDto WorkspaceType { get; set; } = null!;
    public WorkspaceMediaDto[] WorkspaceMedias { get; set; } = null!;
}
