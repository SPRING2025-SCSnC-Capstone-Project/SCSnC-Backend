using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Topping : BaseEntity
{
    public string ToppingName { get; set; }
    public string ToppingDescription { get; set; }
    public double Price { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual IncludeTopping IncludeTopping { get; set; }
}