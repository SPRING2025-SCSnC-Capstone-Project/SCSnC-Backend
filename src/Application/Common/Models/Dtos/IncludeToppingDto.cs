using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class IncludeToppingDto: BaseDto, IMapFrom<IncludeTopping>
{
    public Guid ToppingId { get; set; }
    public Guid OrderDetailId { get; set; }
    public ToppingDto Topping { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<IncludeTopping, IncludeToppingDto>()
            .ForMember(dest => dest.Topping, opt => opt.MapFrom(src => src.Topping))
            .ReverseMap();
    }
}