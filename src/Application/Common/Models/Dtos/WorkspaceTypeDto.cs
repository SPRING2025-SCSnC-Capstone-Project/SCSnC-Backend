using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class WorkspaceTypeDto : BaseDto, IMapFrom<WorkspaceType> {
    public string WorkspaceTypeName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }

}