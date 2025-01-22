using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Voucher
{
    public Voucher()
    {
        Orders = new HashSet<Order>();
        UserVouchers = new HashSet<UserVoucher>();
    }
    
    [Key]
    public Guid VoucherId { get; set; }
    public string VoucherCode { get; set; }
    public int DiscountValue { get; set; }
    public string Description { get; set; }
    public DateTime ExpiredDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<UserVoucher> UserVouchers { get; set; }
}