using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
        Transactions = new HashSet<Transaction>();
    }
    public double TotalPrice { get; set; }
    [ForeignKey("TableId")]
    public Guid? TableId { get; set; }
    [ForeignKey("WorkspaceId")]
    public Guid? WorkspaceId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("VoucherId")]
    public Guid? VoucherId { get; set; }
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public bool PaymentStatus { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Table Table { get; set; }
    public virtual Workspace Workspace { get; set; }
    public virtual User User { get; set; }
    public virtual Voucher? Voucher { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Feedback Feedback { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
}
