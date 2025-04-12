using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Slot : BaseEntity
{
    public Slot()
    {
        ReservedSlots = new HashSet<ReservedSlot>();
        EventSlots = new HashSet<EventSlot>();
    }
    public int SlotNumber { get; set; }
    public LocalTime TimeStart { get; set; }
    public LocalTime TimeEnd { get; set; }
    public bool IsActive { get; set; }
    
    public virtual ICollection<ReservedSlot> ReservedSlots { get; set; }
    public virtual ICollection<EventSlot> EventSlots { get; set; }
}
