using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ShiftTypeDto : BaseDto, IMapFrom<ShiftType>
{
    public string Name { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public BranchDto Branch { get; set; } = null!;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ShiftType, ShiftTypeDto>()
            .ForMember(d => d.StartTime, opt => opt.MapFrom(s => s.StartTime.ToTimeOnly()))
            .ForMember(d => d.EndTime, opt => opt.MapFrom(s => s.EndTime.ToTimeOnly()));
    }
}