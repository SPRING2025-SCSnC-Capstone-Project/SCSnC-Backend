using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Payment
{
    [Key]
    public Guid PaymentId { get; set; }
    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    
    public virtual Transaction Transaction { get; set; }
}