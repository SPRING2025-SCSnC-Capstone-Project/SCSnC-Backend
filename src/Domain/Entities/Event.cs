namespace Domain.Entities;

public class Event : BaseEntity
{
    public Event()
    {
        JoinEvents = new HashSet<JoinEvent>();
    }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string ImgCover { get; set; }
    public double EntranceFee { get; set; }
    public LocalDateTime EventDate { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
}