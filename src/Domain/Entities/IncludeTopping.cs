using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class IncludeTopping
{
    public IncludeTopping()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }
    
    [Key]
    public Guid IncludeToppingId { get; set; }
    [ForeignKey("ToppingId")]
    public Guid ToppingId { get; set; }
    [ForeignKey("OrderDetailId")]
    public Guid OrderDetailId { get; set; }
    
    public virtual Topping Topping { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}