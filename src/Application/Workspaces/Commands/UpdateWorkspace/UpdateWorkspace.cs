using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Workspaces.Commands;

public record UpdateWorkspaceCommand: IRequest<WorkspaceDto> {
    public Guid Id { get; init; }
    public int WorkspaceNumber { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public string? WorkspaceImageUrl { get; init; }
}

public class UpdateWorkspaceCommandHandler: IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public UpdateWorkspaceCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.Include(x => x.WorkspaceType).FirstOrDefaultAsync(x => x.Id == request.Id  && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.Id} does not exist.");
        }

        var existingWorkspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.WorkspaceNumber == request.WorkspaceNumber && x.IsActive, cancellationToken);

        if (existingWorkspace is not null && workspace.WorkspaceNumber != existingWorkspace.WorkspaceNumber) {
            throw new ConflictException($"Workspace with number {request.WorkspaceNumber} already exists");
        }

        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.Id == request.WorkspaceTypeId && x.IsActive);

        if (workspaceType is null) {
            throw new KeyNotFoundException($"Workspace type with Id {request.WorkspaceTypeId} does not exist");
        }

        workspace.WorkspaceNumber = request.WorkspaceNumber;
        //workspace.WorkspaceImageUrl = request.WorkspaceImageUrl;
        workspace.WorkspaceType = workspaceType;
        workspace.WorkspaceTypeId = request.WorkspaceTypeId;

        _context.Workspaces.Update(workspace);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceDto>(workspace);
    }
}