using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class WorkspaceType
{
    public WorkspaceType()
    {
        Workspaces = new HashSet<Workspace>();
    }
    
    [Key]
    public int WorkspaceTypeId { get; set; }
    public string WorkspaceTypeName { get; set; }
    public int MaxCapacity { get; set; }
    
    public virtual ICollection<Workspace> Workspaces { get; set; }
}