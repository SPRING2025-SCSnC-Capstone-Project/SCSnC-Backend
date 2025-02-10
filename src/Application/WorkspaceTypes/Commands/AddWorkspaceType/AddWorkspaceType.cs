using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.WorkspaceTypes.Commands;

public record AddWorkspaceTypeCommand : IRequest<WorkspaceTypeDto> {
    public string WorkspaceTypeName { get; init; } = null!;
    public int MaxCapacity { get; init; }
}

public class AddWorkspaceTypeComamndHandler : IRequestHandler<AddWorkspaceTypeCommand, WorkspaceTypeDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceTypeDto> Handle(AddWorkspaceTypeCommand request, CancellationToken cancellationToken) {
        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.IsActive && x.WorkspaceTypeName.Equals(request.WorkspaceTypeName), cancellationToken);

        if (workspaceType is not null) {
            throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
        }

        var entity = new WorkspaceType() {
            WorkspaceTypeName = request.WorkspaceTypeName,
            MaxCapacity = request.MaxCapacity,
            IsActive = true,
        };

        var result = await _context.WorkspaceTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(result.Entity);
    }
}