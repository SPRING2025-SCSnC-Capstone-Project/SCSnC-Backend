using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ItemWithSize
{
    [Key]
    public Guid ItemWithSizeId { get; set; }
    [ForeignKey("ItemId")]
    public Guid ItemId { get; set; }
    [ForeignKey("SizeId")]
    public Guid SizeId { get; set; }
    
    public virtual Item Item { get; set; }
    public virtual Size Size { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
}