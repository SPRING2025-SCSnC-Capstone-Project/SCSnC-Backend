using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderDetail : BaseEntity
{
    public OrderDetail()
    {
        IncludeToppings = new HashSet<IncludeTopping>();
    }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    [ForeignKey("ItemWithSizeId")]
    public Guid ItemWithSizeId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual ItemWithSize ItemWithSize { get; set; }
    public virtual ICollection<IncludeTopping> IncludeToppings { get; set; }
}