using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceTypeDto : BaseDto, IMapFrom<WorkspaceType> {
    public string WorkspaceTypeName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }
    public HashSet<WorkspaceMediaDto> WorkspaceMedias { get; set; } = null!;
    public HashSet<WorkspaceUtilityServiceDto> WorkspaceUtilityServices { get; set; } = null!;
    public bool HaveEquipmentForRent {  get; set; }
}