using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class WorkspaceType : BaseEntity
{
    public WorkspaceType()
    {
        //Workspaces = new HashSet<Workspace>();
        WorkspaceMedias = new HashSet<WorkspaceMedia>();
        WorkspaceUtilityServices = new HashSet<WorkspaceUtilityService>();
    }
    public string WorkspaceTypeName { get; set; }
    public int MaxCapacity { get; set; }
    public double PricePerHour { get; set; }
    public bool IsActive { get; set; }
    public bool HaveEquipmentForRent { get; set; }
    public string Description { get; set; }

    //public virtual ICollection<Workspace> Workspaces { get; set; }
    public virtual ICollection<WorkspaceTypeAtBranch> WorkspacesAtBranches { get; set; }
    public virtual ICollection<WorkspaceMedia> WorkspaceMedias { get; set; }
    public virtual ICollection<WorkspaceUtilityService> WorkspaceUtilityServices { get; set; }



}