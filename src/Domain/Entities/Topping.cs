using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Topping
{
    public Topping()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }
    
    [Key]
    public Guid ToppingId { get; set; }
    public string ToppingName { get; set; }
    public string ToppingDescription { get; set; }
    public double Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}