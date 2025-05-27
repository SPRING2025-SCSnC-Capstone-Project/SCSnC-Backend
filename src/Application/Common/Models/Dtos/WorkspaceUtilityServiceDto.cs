using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceUtilityServiceDto : BaseDto, IMapFrom<WorkspaceUtilityService> {
    public UtilityServiceDto UtilityService { get; set; } = null!;
    public bool IsAllowToRent {  get; set; }
    public Guid Id { get; set; }
}
