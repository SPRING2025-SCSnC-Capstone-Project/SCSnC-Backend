using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class UtilityServiceDto : BaseDto, IMapFrom<UtilityService> {
    public string ServiceName { get; set; } = null!;
    public string ServiceImage { get; set; } = null!; 
}
