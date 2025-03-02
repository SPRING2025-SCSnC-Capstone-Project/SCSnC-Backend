using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Models.Dtos;

public class TransactionDto: BaseDto, IMapFrom<Transaction>
{
    public Guid PaymentId { get; set; }
    public string TransactionStatus { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid OrderId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Transaction, TransactionDto>()
            .ForMember(d => d.TransactionDate, opt => opt.MapFrom(s => s.TransactionDate.ToDateTimeUnspecified()));
    }
}

public class DetailTransactionDto: BaseDto, IMapFrom<Transaction>
{
    public Guid PaymentId { get; set; }
    public string TransactionStatus { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid OrderId { get; set; }
    public OrderDto Order { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Transaction, DetailTransactionDto>()
            .ForMember(d => d.TransactionDate, opt => opt.MapFrom(s => s.TransactionDate.ToDateTimeUnspecified()))
            .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Order));
    }
}