using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ToppingDto: BaseDto, IMapFrom<Topping>
{
    public string ToppingName { get; set; }
    public string ToppingDescription { get; set; }
    public double Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Topping, ToppingDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ReverseMap();
    }
}