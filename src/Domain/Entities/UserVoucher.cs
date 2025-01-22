using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserVoucher
{
    [Key]
    public Guid UserVoucherId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("VoucherCode")]
    public string VoucherCode { get; set; }
    public DateTime DateAdded { get; set; }
    
    public virtual User User { get; set; }
    public virtual Voucher Voucher { get; set; }
}