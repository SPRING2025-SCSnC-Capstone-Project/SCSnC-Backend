using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Workspace : BaseEntity
{
    public Workspace()
    {
        Reservations = new HashSet<Reservation>();
    }
    public int WorkspaceNumber { get; set; }
    [ForeignKey("WorkspaceTypeId")]
    public Guid WorkspaceTypeId { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public string? WorkspaceImageUrl { get; set; }
    
    public virtual WorkspaceType WorkspaceType { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}