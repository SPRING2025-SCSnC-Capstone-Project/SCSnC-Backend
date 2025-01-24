using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Table
{
    public Table()
    {
        Orders = new HashSet<Order>();
    }
    
    [Key]
    public Guid TableId { get; set; }
    public int TableNumber { get; set; }
    public int SeatAmount { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
}