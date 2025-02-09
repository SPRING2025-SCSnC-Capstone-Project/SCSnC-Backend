using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.WorkspaceTypes.Commands;

public record RemoveWorkspaceTypeCommand : IRequest<WorkspaceTypeDto> {
    public Guid Id { get; init; }
}

public class RemoveWorkspaceTypeComamndHandler : IRequestHandler<RemoveWorkspaceTypeCommand, WorkspaceTypeDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RemoveWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceTypeDto> Handle(RemoveWorkspaceTypeCommand request, CancellationToken cancellationToken) {
        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.IsActive && x.Id == request.Id, cancellationToken);

        if (workspaceType is null) {
            throw new ConflictException($"Workspace type with name {request.Id} does not exist");
        }

        workspaceType.IsActive = false;

        _context.WorkspaceTypes.Update(workspaceType);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(workspaceType);
    }
}