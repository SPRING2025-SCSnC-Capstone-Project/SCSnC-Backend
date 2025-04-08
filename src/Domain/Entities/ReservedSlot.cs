namespace Domain.Entities;

public class ReservedSlot : BaseEntity {
    public Guid SlotId { get; set; }
    public Guid ReservationId { get; set; }

    public virtual Slot? Slot { get; set; }
    public virtual Reservation? Reservation { get; set; }
}
