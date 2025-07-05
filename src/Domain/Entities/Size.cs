using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Size : BaseEntity
{
    public Size()
    {
        ItemWithSizes = new HashSet<ItemWithSize>();
    }
    public string SizeName { get; set; }
    public double PriceAdjustment { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<ItemWithSize> ItemWithSizes { get; set; }
}