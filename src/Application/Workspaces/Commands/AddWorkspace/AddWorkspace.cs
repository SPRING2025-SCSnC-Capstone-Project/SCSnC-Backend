using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Workspaces.Commands;



public record AddWorkspaceCommand: IRequest<WorkspaceDto> {
    public int WorkspaceNumber { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public List<string> MediaTypes { get; init; } = null!;
    public List<string> MediaUrls { get; init; } = null!;
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

        foreach (var mediaType in request.MediaTypes) {
            if (!mediaType.Trim().ToLower().Equals("3d model") 
                    && !mediaType.Trim().ToLower().Equals("image")) {
                throw new InvalidDataException($"Invalid media type: {mediaType}");
            }
        }

        var entity = new Workspace() {
            WorkspaceNumber = request.WorkspaceNumber,
            IsAvailable = true,
            IsActive = true,
            WorkspaceTypeId = request.WorkspaceTypeId,
        };

        var result = await _context.Workspaces.AddAsync(entity, cancellationToken);
        
        var workspaceMediasToAdd = new List<WorkspaceMedia>();

        for (var i = 0; i < request.MediaTypes.Count; i++) {
            var workspaceMedia = new WorkspaceMedia() {
                WorkspaceId = result.Entity.Id,
                MediaType = request.MediaTypes.ElementAt(i),
                MediaUrl = request.MediaUrls.ElementAt(i),
            };

            workspaceMediasToAdd.Add(workspaceMedia);
        } 

        await _context.WorkspaceMedias.AddRangeAsync(workspaceMediasToAdd, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var addedWorkspace = await _context.Workspaces
            .Include(x => x.WorkspaceMedias)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);

        return _mapper.Map<WorkspaceDto>(addedWorkspace);
    }
}
