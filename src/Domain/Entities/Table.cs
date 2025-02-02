using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Table : BaseEntity
{
    public Table()
    {
        Orders = new HashSet<Order>();
    }
    public int TableNumber { get; set; }
    public int SeatAmount { get; set; }
    public bool IsAvailable { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
}