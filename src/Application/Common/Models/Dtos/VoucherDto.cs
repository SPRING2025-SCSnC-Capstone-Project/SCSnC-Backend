using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class VoucherDto: BaseDto, IMapFrom<Voucher>
{
    public string VoucherCode { get; set; }
    public int DiscountValue { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpiredDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Voucher, VoucherDto>()
            .ForMember(d => d.ExpiredDate, opt => opt.MapFrom(s => s.ExpiredDate.ToDateTimeUnspecified()))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt.ToDateTimeUnspecified()))
            .ForMember(d => d.LastUpdatedAt, opt => opt.MapFrom(s => s.LastUpdatedAt.ToDateTimeUnspecified()));
    }
}