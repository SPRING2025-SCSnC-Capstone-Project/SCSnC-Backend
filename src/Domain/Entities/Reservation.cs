using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Reservation : BaseEntity
{
    public LocalDate ReservationDate { get; set; }
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    //[ForeignKey("SlotId")]
    //public Guid SlotId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public LocalTime StartTime { get; set; }
    public LocalTime EndTime { get; set; }
    public bool IsFullPaid{ get; set; }
    public double TotalPrice { get; set; }
    // public LocalDateTime CreatedAt { get; set; }
    // public LocalDateTime LastUpdatedAt { get; set; }
        // may need reviews to add these fields
    
    public virtual Workspace Workspace { get; set; }
    public virtual User User { get; set; }
}
