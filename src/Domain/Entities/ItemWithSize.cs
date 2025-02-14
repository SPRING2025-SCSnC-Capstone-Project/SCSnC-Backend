using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ItemWithSize : BaseEntity
{
    public ItemWithSize()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }
    
    [ForeignKey("ItemId")]
    public Guid ItemId { get; set; }
    [ForeignKey("SizeId")]
    public Guid SizeId { get; set; }
    public bool IsActive { get; set; }
    
    public virtual Item Item { get; set; }
    public virtual Size Size { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}