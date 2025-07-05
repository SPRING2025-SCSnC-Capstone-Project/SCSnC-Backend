namespace Domain.Entities;

public class Feedback : BaseEntity
{
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Order Order { get; set; }
}