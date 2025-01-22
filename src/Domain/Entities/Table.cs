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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //This line is for AutoIncrement
    public int TableId { get; set; }
    public int SeatAmount { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }
}