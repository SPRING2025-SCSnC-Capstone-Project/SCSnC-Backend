using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Slot : BaseEntity
{
    public Slot()
    {
        Reservations = new HashSet<Reservation>();
    }
    public int SlotNumber { get; set; }
    public LocalTime TimeStart { get; set; }
    public LocalTime TimeEnd { get; set; }
    public bool IsActive { get; set; }
    
    public virtual ICollection<Reservation> Reservations { get; set; }
}