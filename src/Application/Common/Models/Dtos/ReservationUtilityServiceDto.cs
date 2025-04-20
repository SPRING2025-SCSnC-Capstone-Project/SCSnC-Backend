using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ReservationUtilityServiceDto : BaseDto, IMapFrom<ReservationUtilityService> {
    public WorkspaceUtilityServiceDto WorkspaceUtilityService { get; set; } = null;
}
