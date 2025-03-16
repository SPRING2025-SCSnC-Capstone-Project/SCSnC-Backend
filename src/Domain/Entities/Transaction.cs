using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public string TransactionStatus { get; set; }
    public LocalDateTime TransactionDate { get; set; }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    
    public virtual Order Order { get; set; }
}