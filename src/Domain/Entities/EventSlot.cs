namespace Domain.Entities;

public class EventSlot : BaseEntity {
    public Guid SlotId { get; set; }
    public Guid EventId { get; set; }

    public virtual Slot? Slot { get; set; }
    public virtual Event? Event { get; set; }
}
