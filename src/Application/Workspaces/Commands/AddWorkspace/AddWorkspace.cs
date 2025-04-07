using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Workspaces.Commands;

public record AddWorkspaceCommand: IRequest<WorkspaceDto> {
    public int WorkspaceNumber { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public string? WorkspaceImageUrl { get; init; }
    public string Name { get; init; }
}

public class AddWorkspaceCommandHandler: IRequestHandler<AddWorkspaceCommand, WorkspaceDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddWorkspaceCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(AddWorkspaceCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.WorkspaceNumber == request.WorkspaceNumber && x.IsActive, cancellationToken);

        if (workspace is not null) {
            throw new ConflictException($"Workspace with number {request.WorkspaceNumber} already exists");
        }

        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.Id == request.WorkspaceTypeId && x.IsActive, cancellationToken);

        if (workspaceType is null) {
            throw new KeyNotFoundException($"Workspace type with Id {request.WorkspaceTypeId} does not exists");
        }

        var entity = new Workspace() {
            WorkspaceNumber = request.WorkspaceNumber,
            Name = request.Name,
            IsAvailable = true,
            IsActive = true,
            //WorkspaceImageUrl = request.WorkspaceImageUrl,
            WorkspaceTypeId = request.WorkspaceTypeId,
        };

        var result = await _context.Workspaces.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceDto>(result.Entity);
    }
}
