using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.WorkspaceTypes.Commands;

public record AddWorkspaceTypeCommand : IRequest<WorkspaceTypeDto> {
    public string WorkspaceTypeName { get; init; } = null!;
    public int MaxCapacity { get; init; }
    public double PricePerHour { get; set; }
    public List<string> MediaTypes { get; init; } = null!;
    public List<string> MediaUrls { get; init; } = null!;
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
        var addedWorkspaceType = new WorkspaceType();

        if (workspaceType is not null) {
            throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
        }
        foreach (var mediaType in request.MediaTypes)
        {
            if (!mediaType.Trim().ToLower().Equals("3d model")
                    && !mediaType.Trim().ToLower().Equals("image"))
            {
                throw new InvalidDataException($"Invalid media type: {mediaType}");
            }
        }

        var entity = new WorkspaceType() {
            WorkspaceTypeName = request.WorkspaceTypeName,
            MaxCapacity = request.MaxCapacity,
            PricePerHour = request.PricePerHour,
            IsActive = true,
        };
        var result = await _context.WorkspaceTypes.AddAsync(entity, cancellationToken);
        var workspaceMediasToAdd = new List<WorkspaceMedia>();

        for (var i = 0; i < request.MediaTypes.Count; i++)
        {
            var workspaceMedia = new WorkspaceMedia()
            {
                WorkspaceTypeId = result.Entity.Id,
                MediaType = request.MediaTypes.ElementAt(i),
                MediaUrl = request.MediaUrls.ElementAt(i),
            };

            workspaceMediasToAdd.Add(workspaceMedia);
        }
        await _context.WorkspaceMedias.AddRangeAsync(workspaceMediasToAdd, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        addedWorkspaceType = await _context.WorkspaceTypes.Include(x => x.WorkspaceMedias).FirstOrDefaultAsync(x => x.Id.Equals(result.Entity.Id), cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(result.Entity);
    }
}