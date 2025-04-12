namespace Domain.Entities;

public class Event : BaseEntity
{
    public Event()
    {
        JoinEvents = new HashSet<JoinEvent>();
        Blogs = new HashSet<Blog>();
        EventSlots = new HashSet<EventSlot>();
    }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public LocalDate EventDate { get; set; }
    public string CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    [ForeignKey("ReservationId")]
    public Guid ReservationId { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }

    public string Status { get; set; }
    public bool IsActive { get; set; }

    public virtual Reservation Reservation { get; set; }
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
    public virtual ICollection<Blog> Blogs { get; set; }
    public virtual ICollection<EventSlot> EventSlots { get; set; }
}
