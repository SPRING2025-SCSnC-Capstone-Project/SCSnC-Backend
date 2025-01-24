using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    [ForeignKey("SlotId")]
    public Guid SlotId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    // public bool Status { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public DateTime LastUpdatedAt { get; set; }
        // may need reviews to add these fields
    
    public virtual Workspace Workspace { get; set; }
    public virtual Slot Slot { get; set; }
    public virtual User User { get; set; }
}