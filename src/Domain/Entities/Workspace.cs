using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Workspace
{
    [Key]
    public int WorkspaceId { get; set; }
    public bool IsAvailable { get; set; }
    [ForeignKey("WorkspaceTypeId")]
    public int WorkspaceTypeId { get; set; }
    
    public virtual WorkspaceType WorkspaceType { get; set; }
}