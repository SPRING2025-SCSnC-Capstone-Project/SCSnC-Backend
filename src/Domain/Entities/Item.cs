using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Item
{
    public Item()
    {
        Sizes = new HashSet<Size>();
    }
    
    [Key]
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public double ItemBasePrice { get; set; }
    public string ItemImg { get; set; }
    [ForeignKey("ItemCategoryId")]
    public Guid ItemCategoryId { get; set; }
    [ForeignKey("SizeId")]
    public Guid SizeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ItemCategory ItemCategory { get; set; }
    public virtual ICollection<Size> Sizes { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
}