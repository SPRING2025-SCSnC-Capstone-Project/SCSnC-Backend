using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Workspace : BaseEntity
{
    public Workspace()
    {
        Reservations = new HashSet<Reservation>();
        Orders = new HashSet<Order>();
        WorkspaceMedias = new HashSet<WorkspaceMedia>();
        WorkspaceUtilityServices = new HashSet<WorkspaceUtilityService>();
    }
    public int WorkspaceNumber { get; set; }
    [ForeignKey("WorkspaceTypeId")]
    public Guid WorkspaceTypeId { get; set; }
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public string WorkspaceName { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public double PricePerHour { get; set; }
    
    public virtual WorkspaceType WorkspaceType { get; set; } = null!;
    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<WorkspaceMedia> WorkspaceMedias { get; set; }
    public virtual ICollection<WorkspaceUtilityService> WorkspaceUtilityServices { get; set; }
}
