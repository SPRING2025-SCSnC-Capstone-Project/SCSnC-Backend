using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class UserVoucherDto: BaseDto, IMapFrom<UserVoucher>
{
    public Guid UserId { get; set; }
    public VoucherDto Voucher { get; set; }
    public DateTime DateAdded { get; set; }
    public bool RedeemStatus { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserVoucher, UserVoucherDto>()
            .ForMember(d => d.DateAdded, opt => opt.MapFrom(s => s.DateAdded.ToDateTimeUnspecified()))
            .ForMember(d => d.Voucher, opt => opt.MapFrom(s => s.Voucher));
    }
}