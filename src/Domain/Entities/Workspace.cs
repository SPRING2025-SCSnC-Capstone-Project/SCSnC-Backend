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
        Events = new HashSet<Event>();
    }
    public int WorkspaceNumber { get; set; }
    [ForeignKey("WorkspaceTypeId")]
    public Guid WorkspaceTypeId { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsActive { get; set; }
    public double PricePerHour { get; set; }
    
    public virtual WorkspaceType WorkspaceType { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<WorkspaceMedia> WorkspaceMedias { get; set; }
    public virtual ICollection<Event> Events { get; set; }
}
