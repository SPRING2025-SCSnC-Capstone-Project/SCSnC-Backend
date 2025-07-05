using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserVoucher : BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("VoucherId")]
    public Guid VoucherId { get; set; }
    public LocalDateTime DateAdded { get; set; }
    public bool RedeemStatus { get; set; }
    
    public virtual User User { get; set; }
    public virtual Voucher Voucher { get; set; }
}