using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ItemCategory : BaseEntity
{
    public ItemCategory()
    {
        Items = new HashSet<Item>();
    }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Item> Items { get; set; }
}