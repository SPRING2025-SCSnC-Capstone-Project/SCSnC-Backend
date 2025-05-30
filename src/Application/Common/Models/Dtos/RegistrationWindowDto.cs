using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class RegistrationWindowDto: BaseDto, IMapFrom<RegistrationWindow>
{
    public string BranchName { get; set; }
    public DateOnly WeekStart { get; set; }
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<RegistrationWindow, RegistrationWindowDto>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
            .ForMember(dest => dest.WeekStart, opt => opt.MapFrom(src => src.WeekStart.ToDateOnly()))
            .ForMember(dest => dest.OpenAt, opt => opt.MapFrom(src => src.OpenAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.CloseAt, opt => opt.MapFrom(src => src.CloseAt.ToDateTimeUnspecified()));
    }
}

public class DeleteRegistrationWindowResponse
{
    public string Message { get; set; }
}