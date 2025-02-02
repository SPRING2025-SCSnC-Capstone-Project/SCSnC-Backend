using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class SizeDto: BaseDto, IMapFrom<Size>
{
    public string SizeName { get; set; }
    public double PriceAdjustment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Size, SizeDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ReverseMap();
    }
}