using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ItemCategory
{
    public ItemCategory()
    {
        Items = new HashSet<Item>();
    }
    
    [Key]
    public Guid ItemCategoryId { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Item> Items { get; set; }
}