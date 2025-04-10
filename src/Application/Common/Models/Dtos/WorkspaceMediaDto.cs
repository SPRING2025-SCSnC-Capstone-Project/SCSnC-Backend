using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceMediaDto : BaseDto, IMapFrom<WorkspaceMedia> {
    public string MediaType { get; set; } = null!;
    public string MediaUrl { get; set; } = null!;
}
