using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    [ForeignKey("PaymentId")]
    public Guid PaymentId { get; set; }
    public string TransactionStatus { get; set; }
    public LocalDateTime TransactionDate { get; set; }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    
    public virtual Payment Payment { get; set; }
    public virtual Order Order { get; set; }
}