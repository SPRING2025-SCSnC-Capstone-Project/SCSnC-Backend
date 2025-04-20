using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Reservation : BaseEntity
{
    public Reservation() {
        
        ReservedSlots = new HashSet<ReservedSlot>();
        Transactions = new HashSet<Transaction>();
        
    }

    public LocalDate ReserveDate { get; set; }
    [ForeignKey("WorkspaceId")]
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    [ForeignKey("UserId")]
    public Guid? UserId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool IsFullPaid{ get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    // public LocalDateTime CreatedAt { get; set; }
    // public LocalDateTime LastUpdatedAt { get; set; }
        // may need reviews to add these fields
    
    public virtual Workspace Workspace { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<ReservedSlot> ReservedSlots { get; set; } 
    public virtual ICollection<Transaction> Transactions { get; set; }
    public virtual ICollection<ReservationUtilityService> ReservationUtilityServices { get; }
}
