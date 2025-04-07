using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class SingleItemCategoryDto: BaseDto, IMapFrom<ItemCategory>
{
    public string CategoryName { get; set; }
    //public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public List<ItemDto> Items { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ItemCategory, SingleItemCategoryDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt.ToDateTimeUnspecified()));
    }
}

public class ItemCategoryDto: BaseDto, IMapFrom<ItemCategory>
{
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ItemCategory, ItemCategoryDto>()
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt.ToDateTimeUnspecified()));
    }
}