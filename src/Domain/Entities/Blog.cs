namespace Domain.Entities;

public class Blog: BaseEntity
{
    public Blog()
    {
        BlogMedias = new HashSet<BlogMedia>();
    }
    [ForeignKey("EventId")]
    public Guid EventId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Event Event { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<BlogMedia> BlogMedias { get; set; }
}