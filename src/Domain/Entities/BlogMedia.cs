namespace Domain.Entities;

public class BlogMedia: BaseEntity
{
    [ForeignKey("BlogId")]
    public Guid BlogId { get; set; }
    public string MediaType { get; set; }
    public string MediaUrl { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Blog Blog { get; set; }
}