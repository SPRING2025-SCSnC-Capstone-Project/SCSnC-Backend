using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Event
{
    public Event()
    {
        JoinEvents = new HashSet<JoinEvent>();
    }
    
    [Key]
    public Guid EventId { get; set; }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string ImgCover { get; set; }
    public double EntranceFee { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
}