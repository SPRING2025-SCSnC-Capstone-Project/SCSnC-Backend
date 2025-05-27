using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkspaceTypeAtBranch: BaseEntity
    {
        public WorkspaceTypeAtBranch()
        {
            Workspaces = new HashSet<Workspace>();
        }
        [ForeignKey("WorkspaceTypeId")]
        public Guid WorkspaceTypeId { get; set; }
        [ForeignKey("BranchId")]
        public Guid BranchId { get; set; }
        public bool IsAvailable { get; set; }
        public LocalDateTime CreatedAt { get; set; }
        public LocalDateTime LastUpdatedAt { get; set; }

        public virtual WorkspaceType WorkspaceType { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ICollection<Workspace> Workspaces { get; set; }
    }
}
