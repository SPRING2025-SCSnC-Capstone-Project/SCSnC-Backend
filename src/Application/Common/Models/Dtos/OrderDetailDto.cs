using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class OrderDetailDto: BaseDto, IMapFrom<OrderDetail>
{
    public string ItemName { get; set; }
    public string SizeName { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    public List<string> Toppings { get; set; }
    public string Additional { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemWithSize.Item.ItemName))
            .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ItemWithSize.Size.SizeName))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.IncludeToppings != null ? src.IncludeToppings.Where(i => i.OrderDetailId == src.Id).Select(i => i.Topping.ToppingName).ToList() : new List<string>()))
            .ReverseMap();
    }
}

public class CreateOrderDetailDto
{
    public Guid ItemId { get; set; }
    public Guid SizeId { get; set; }
    public int Quantity { get; set; }
    public List<Guid> ToppingIds { get; set; }
    public double OrderDetailPrice { get; set; }
    public string Additional { get; set; }

}