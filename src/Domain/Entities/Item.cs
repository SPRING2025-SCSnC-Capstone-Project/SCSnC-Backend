using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Item
{
    public Item()
    {
        ItemWithSizes = new HashSet<ItemWithSize>();
    }
    
    [Key]
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public double ItemBasePrice { get; set; }
    public string ItemImg { get; set; }
    [ForeignKey("ItemCategoryId")]
    public Guid ItemCategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ItemCategory ItemCategory { get; set; }
    public virtual ICollection<ItemWithSize> ItemWithSizes { get; set; }
}