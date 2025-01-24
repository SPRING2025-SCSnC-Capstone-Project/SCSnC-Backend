using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Slot
{
    public Slot()
    {
        Reservations = new HashSet<Reservation>();
    }
    
    [Key]
    public Guid SlotId { get; set; }
    public int SlotNumber { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
    
    public virtual ICollection<Reservation> Reservations { get; set; }
}