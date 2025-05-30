using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public string TransactionStatus { get; set; }
    public LocalDateTime TransactionDate { get; set; }
    [ForeignKey("OrderId")]
    public Guid? OrderId { get; set; }
    [ForeignKey("ReservationId")]
    public Guid? ReservationId { get; set; }
    public double Amount { get; set; }
    public string TypeOfPayment { get; set; }
    public string PaymentMethod { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual Reservation Reservation { get; set; }
}