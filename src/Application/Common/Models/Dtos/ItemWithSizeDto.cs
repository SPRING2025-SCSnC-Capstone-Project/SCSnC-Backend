using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class ItemWithSizeDto: BaseDto, IMapFrom<ItemWithSize>
{
    public Guid ItemId { get; set; }
    public Guid SizeId { get; set; }
    public bool IsActive { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ItemWithSizeDto, ItemWithSize>().ReverseMap();
    }
}