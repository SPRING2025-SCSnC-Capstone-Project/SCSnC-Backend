using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Workspaces.Commands;

public record UpdateWorkspaceCommand: IRequest<WorkspaceDto> {
    public Guid Id { get; init; }
    public Guid BranchId { get; init; }
    public int WorkspaceNumber { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public string WorkspaceName { get; init; } = null!;
    public string Description { get; init; } = null!;

}

public class UpdateWorkspaceCommandHandler: IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public UpdateWorkspaceCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.Include(x => x.WorkspaceTypeAtBranch.WorkspaceType).FirstOrDefaultAsync(x => x.Id == request.Id  && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.Id} does not exist.");
        }

        var existingWorkspaceNumber = await _context.Workspaces.Include(x => x.WorkspaceTypeAtBranch).FirstOrDefaultAsync(x => x.WorkspaceNumber == request.WorkspaceNumber 
                && x.WorkspaceTypeAtBranch.Branch.Id == request.BranchId
                && x.IsActive, cancellationToken);

        if (existingWorkspaceNumber is not null && workspace.WorkspaceNumber != existingWorkspaceNumber.WorkspaceNumber) {
            throw new ConflictException($"Workspace with number {request.WorkspaceNumber} already exists");
        }

        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.Id == request.WorkspaceTypeId && x.IsActive, cancellationToken);

        if (workspaceType is null) {
            throw new KeyNotFoundException($"Workspace type with Id {request.WorkspaceTypeId} does not exist");
        }

        var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == request.BranchId
                && x.IsActive, cancellationToken);

        if (branch is null) {
            throw new KeyNotFoundException($"Branch with Id {request.BranchId} does not exist");
        }

        workspace.WorkspaceNumber = request.WorkspaceNumber;
        workspace.WorkspaceTypeAtBranch.WorkspaceType = workspaceType;
        workspace.WorkspaceTypeAtBranch.WorkspaceTypeId = request.WorkspaceTypeId;

        _context.Workspaces.Update(workspace);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceDto>(workspace);
    }
}
