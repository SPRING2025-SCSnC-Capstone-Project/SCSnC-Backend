using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceDto : BaseDto, IMapFrom<Workspace>
{
    public int WorkspaceNumber { get; set; }
    public double PricePerHour { get; set; }
    public bool IsAvailable { get; set; }
    public WorkspaceTypeDto WorkspaceType { get; set; } = null!;
    public BranchDto Branch { get; set; } = null!;
    public WorkspaceMediaDto[] WorkspaceMedias { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.WorkspaceType, opt => opt.MapFrom(src => src.WorkspaceTypeAtBranch.WorkspaceType))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.WorkspaceTypeAtBranch.Branch))
            .ReverseMap();
    }
}
