using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ShiftTypeDto: BaseDto, IMapFrom<ShiftType>
{
    public Guid BranchId { get; set; }
    public string Name { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ShiftType, ShiftTypeDto>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToTimeOnly()))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToTimeOnly()));
    }
}

public class ReturnShiftTypeDto: BaseDto, IMapFrom<ShiftType>
{
    public string BranchName { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ShiftType, ReturnShiftTypeDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToTimeOnly()))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToTimeOnly()));
    }
}