using Application.Common.Mappings;
using Domain.Common;
using Domain.Entities;
using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Dtos;
public class WorkspaceTypeAtBranchDto : BaseDto, IMapFrom<WorkspaceTypeAtBranch>
{
    public Guid WorkspaceTypeId { get; set; }
    public Guid BranchId { get; set; }
    public bool IsAvailable { get; set; }
    public WorkspaceTypeDto WorkspaceType { get; set; } = null!;
    public BranchDto Branch { get; set; } = null!;
    public HashSet<WorkspaceDto> Workspaces { get; set; } = null!;

}
