using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Feedback
{
    [Key]
    public Guid FeedbackId { get; set; }
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual Order Order { get; set; }
}