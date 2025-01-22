using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
        Transactions = new HashSet<Transaction>();
    }
    
    [Key]
    public Guid OrderId { get; set; }
    public double TotalPrice { get; set; }
    [ForeignKey("TableId")]
    public Guid TableId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("VoucherCode")]
    public Guid VoucherId { get; set; }
    public bool PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual Table Table { get; set; }
    public virtual User User { get; set; }
    public virtual Voucher Voucher { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Feedback Feedback { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
}