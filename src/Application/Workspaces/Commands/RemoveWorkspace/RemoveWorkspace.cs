using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Workspaces.Commands;

public record RemoveWorkspaceCommand: IRequest<WorkspaceDto> {
    public Guid Id { get; init; }
}

public class RemoveWorkspaceCommandHandler: IRequestHandler<RemoveWorkspaceCommand, WorkspaceDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public RemoveWorkspaceCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(RemoveWorkspaceCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.Include(x => x.WorkspaceTypeAtBranch.WorkspaceType).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.Id} does not exist");
        }

        workspace.IsActive = false;

        _context.Workspaces.Update(workspace);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceDto>(workspace);
    }
}