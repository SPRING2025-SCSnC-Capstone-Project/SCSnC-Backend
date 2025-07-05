using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Item : BaseEntity
{
    public Item()
    {
        ItemWithSizes = new HashSet<ItemWithSize>();
        ItemPricesAtBranches = new HashSet<ItemPriceAtBranch>();
    }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public string ItemImg { get; set; }
    [ForeignKey("ItemCategoryId")]
    public Guid ItemCategoryId { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ItemCategory ItemCategory { get; set; }
    public virtual ICollection<ItemWithSize> ItemWithSizes { get; set; }
    public virtual ICollection<ItemPriceAtBranch> ItemPricesAtBranches { get; set; }
}