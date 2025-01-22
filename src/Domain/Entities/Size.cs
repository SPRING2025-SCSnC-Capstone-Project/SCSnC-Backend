using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Size
{
    public Size()
    {
        Items = new HashSet<Item>();
    }
    
    [Key]
    public Guid SizeId { get; set; }
    public string SizeName { get; set; }
    public double PriceAdjustment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Item> Items { get; set; }
}