using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class OrderDto: BaseDto, IMapFrom<Order>
{
    public string PaymentLink { get; set; }
    public Guid UserId { get; set; }
    public double TotalPrice { get; set; }
    public int TableNumber { get; set; }
    public string? VoucherCode { get; set; }
    public bool PaymentStatus { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber))
            .ForMember(dest => dest.VoucherCode, opt => opt.MapFrom(src => src.Voucher.VoucherCode))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ReverseMap();
    }
}

public class ResponseOrderDto: BaseDto, IMapFrom<Order>
{
    public Guid UserId { get; set; }
    public double TotalPrice { get; set; }
    public int TableNumber { get; set; }
    public string? VoucherCode { get; set; }
    public bool PaymentStatus { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, ResponseOrderDto>()
            .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber))
            .ForMember(dest => dest.VoucherCode, opt => opt.MapFrom(src => src.Voucher.VoucherCode))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(dest => dest.LastUpdatedAt, opt => opt.MapFrom(src => src.LastUpdatedAt.ToDateTimeUnspecified()))
            .ReverseMap();
    }
}