using Application.Common.Mappings;
using Domain.Entities;
using NodaTime;

namespace Application.Common.Models.Dtos;

public class ItemDto : BaseDto, IMapFrom<Item>
{
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public double ItemBasePrice { get; set; }
    public string ItemImg { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public List<SizeDto> Sizes { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Item, ItemDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ItemCategory.CategoryName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.ItemWithSizes.Select(iws => iws.Size).ToList()))
            .ReverseMap();
    }
}