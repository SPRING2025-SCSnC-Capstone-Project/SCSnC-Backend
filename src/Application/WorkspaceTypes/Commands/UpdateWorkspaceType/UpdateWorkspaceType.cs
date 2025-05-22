using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.WorkspaceTypes.Commands;

public record UpdateWorkspaceTypeCommand : IRequest<WorkspaceTypeDto> {
    public Guid Id { get; init; }
    public string? WorkspaceTypeName { get; init; } = null!;
    public int? MaxCapacity { get; init; }
    public double? PricePerHour { get; set; }
    public WorkspaceUtilityServiceDto[] WorkspaceUtilityServices { get; set; }
    public IFormFile? ModelFile { get; set; }
}

public class UpdateWorkspaceTypeComamndHandler : IRequestHandler<UpdateWorkspaceTypeCommand, WorkspaceTypeDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public UpdateWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper, IAzureService azureService = null)
    {
        _context = context;
        _mapper = mapper;
        _azureService = azureService;
    }

    public async Task<WorkspaceTypeDto> Handle(UpdateWorkspaceTypeCommand request, CancellationToken cancellationToken) {
        var workspaceType = await _context.WorkspaceTypes.Include(x => x.WorkspaceUtilityServices).FirstOrDefaultAsync(x => x.IsActive && x.Id == request.Id, cancellationToken);

        if (workspaceType is null) {
            throw new ConflictException($"Workspace type with name {request.Id} does not exist");
        }

        var existingWorkspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.WorkspaceTypeName == request.WorkspaceTypeName && x.IsActive, cancellationToken);

        if (existingWorkspaceType is not null && workspaceType.WorkspaceTypeName != existingWorkspaceType.WorkspaceTypeName) {
            throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
        }

        workspaceType.WorkspaceTypeName = request.WorkspaceTypeName ?? workspaceType.WorkspaceTypeName;
        workspaceType.MaxCapacity = request.MaxCapacity ?? workspaceType.MaxCapacity;

        foreach(var utilityService in request.WorkspaceUtilityServices)
        {
            workspaceType.WorkspaceUtilityServices.FirstOrDefault(x => x.Id == utilityService.Id).IsAllowToRent = utilityService.IsAllowToRent;
        }

        var modelUrl = "";
        if (request.ModelFile != null || request.ModelFile.Length > 0)
        {
            modelUrl = await _azureService.UploadModel(request.ModelFile);
        }

        _context.WorkspaceTypes.Update(workspaceType);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(workspaceType);
    }
}