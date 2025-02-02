using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Payment : BaseEntity
{
    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    
    public virtual Transaction Transaction { get; set; }
}