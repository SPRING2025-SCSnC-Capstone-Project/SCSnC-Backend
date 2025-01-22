using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class OrderDetail
{
    public OrderDetail()
    {
        Toppings = new HashSet<Topping>();
    }
    
    [Key]
    public Guid OrderDetailId { get; set; }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    [ForeignKey("ItemId")]
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual Item Item { get; set; }
    public virtual ICollection<Topping> Toppings { get; set; }
}