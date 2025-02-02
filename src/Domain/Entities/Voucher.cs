using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Voucher : BaseEntity
{
    public Voucher()
    {
        Orders = new HashSet<Order>();
        UserVouchers = new HashSet<UserVoucher>();
    }
    public string VoucherCode { get; set; }
    public int DiscountValue { get; set; }
    public string Description { get; set; }
    public LocalDateTime ExpiredDate { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<UserVoucher> UserVouchers { get; set; }
}