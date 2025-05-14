using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceUtilityServiceDto : BaseDto, IMapFrom<WorkspaceUtilityService> {
    public double ServiceFee { get; set; }
    public UtilityServiceDto UtilityService { get; set; } = null!;
}
