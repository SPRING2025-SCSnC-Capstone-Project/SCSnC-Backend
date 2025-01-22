using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class JoinEvent
{
    [Key]
    public Guid JoinEventId { get; set; }
    [ForeignKey("EventId")]
    public Guid EventId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public double Deposit { get; set; }
    public bool IsFullPaid { get; set; }
    public bool IsAttended { get; set; }
    
    public virtual Event Event { get; set; }
    public virtual User User { get; set; }
}