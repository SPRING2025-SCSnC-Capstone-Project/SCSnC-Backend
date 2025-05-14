using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Workspace : BaseEntity
{
    public Workspace()
    {
        Reservations = new HashSet<Reservation>();
        Orders = new HashSet<Order>();
        WorkspaceUtilityServices = new HashSet<WorkspaceUtilityService>();
    }
    public int WorkspaceNumber { get; set; }
    [ForeignKey("WorkspaceTypeAtBranchId")]
    public Guid WorkspaceTypeAtBranchId { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public double PricePerHour { get; set; }
    
    public virtual WorkspaceTypeAtBranch WorkspaceTypeAtBranch { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<WorkspaceUtilityService> WorkspaceUtilityServices { get; set; }
}
