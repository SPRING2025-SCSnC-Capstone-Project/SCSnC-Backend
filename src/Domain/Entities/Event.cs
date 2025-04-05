namespace Domain.Entities;

public class Event : BaseEntity
{
    public Event()
    {
        JoinEvents = new HashSet<JoinEvent>();
        Blogs = new HashSet<Blog>();
    }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public LocalDateTime EventStartDate { get; set; }
    public LocalDateTime EventEndDate { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
    
    public virtual Workspace Workspace { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
    public virtual ICollection<Blog> Blogs { get; set; }
}
