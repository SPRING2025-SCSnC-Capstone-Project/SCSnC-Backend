using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.WorkspaceTypes.Commands;

public record UpdateWorkspaceTypeCommand : IRequest<WorkspaceTypeDto> {
    public Guid Id { get; init; }
    public string WorkspaceTypeName { get; init; } = null!;
    public int MaxCapacity { get; init; }
}

public class UpdateWorkspaceTypeComamndHandler : IRequestHandler<UpdateWorkspaceTypeCommand, WorkspaceTypeDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceTypeDto> Handle(UpdateWorkspaceTypeCommand request, CancellationToken cancellationToken) {
        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.IsActive && x.Id == request.Id, cancellationToken);

        if (workspaceType is null) {
            throw new ConflictException($"Workspace type with name {request.Id} does not exist");
        }

        var existingWorkspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.WorkspaceTypeName == request.WorkspaceTypeName && x.IsActive, cancellationToken);

        if (existingWorkspaceType is not null && workspaceType.WorkspaceTypeName != existingWorkspaceType.WorkspaceTypeName) {
            throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
        }

        workspaceType.WorkspaceTypeName = request.WorkspaceTypeName;
        workspaceType.MaxCapacity = request.MaxCapacity;

        _context.WorkspaceTypes.Update(workspaceType);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(workspaceType);
    }
}