using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Application.WorkspaceTypes.Commands;

public class UpdateWorkspaceTypeImage
{
    public string name { get; set; }
    public string oldSrc { get; set; }
    public string newImageName { get; set; }
}

public record UpdateWorkspaceTypeCommand : IRequest<WorkspaceTypeDto>
{
    public Guid Id { get; init; }
    public string? WorkspaceTypeName { get; init; } = null!;
    public string? WorkspaceTypeDescription { get; init; } = null!;
    public int? MaxCapacity { get; init; }
    public double? PricePerHour { get; set; }
    public bool IsActive { get; set; }
    public WorkspaceUtilityServiceDto[] WorkspaceUtilityServices { get; set; }
    public WorkspaceTypeAtBranchDto[] WorkspacesAtBranches { get; set; }
    public WorkspaceInWorkspaceType[] WorkspaceInWorkspaceTypes { get; init; } = null!;
    public UpdateWorkspaceTypeImage[] UpdateWorkspaceTypeImages { get; set; }
    public IFormFile? ModelFile { get; set; }
    public IFormFile[]? NewImages { get; set; }
}

public class UpdateWorkspaceTypeComamndHandler : IRequestHandler<UpdateWorkspaceTypeCommand, WorkspaceTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public UpdateWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper, IAzureService azureService = null)
    {
        _context = context;
        _mapper = mapper;
        _azureService = azureService;
    }

    public async Task<WorkspaceTypeDto> Handle(UpdateWorkspaceTypeCommand request, CancellationToken cancellationToken)
    {
        var workspaceType = await _context.WorkspaceTypes
            .Include(x => x.WorkspaceUtilityServices)
            .Include(x => x.WorkspacesAtBranches)
                .ThenInclude(y => y.Branch)
            .Include(x => x.WorkspacesAtBranches)
                .ThenInclude(y => y.Workspaces)
            .FirstOrDefaultAsync(x => x.IsActive && x.Id == request.Id, cancellationToken);

        if (workspaceType is null)
        {
            throw new ConflictException($"Workspace type with name {request.Id} does not exist");
        }

        var existingWorkspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.WorkspaceTypeName == request.WorkspaceTypeName && x.IsActive, cancellationToken);

        if (existingWorkspaceType is not null && workspaceType.WorkspaceTypeName != existingWorkspaceType.WorkspaceTypeName)
        {
            throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
        }

        workspaceType.WorkspaceTypeName = request.WorkspaceTypeName ?? workspaceType.WorkspaceTypeName;
        workspaceType.MaxCapacity = request.MaxCapacity ?? workspaceType.MaxCapacity;
        workspaceType.IsActive = request.IsActive;
        workspaceType.PricePerHour = request.PricePerHour ?? workspaceType.PricePerHour;
        workspaceType.Description = request.WorkspaceTypeDescription ?? workspaceType.Description;

        foreach (var utilityService in request.WorkspaceUtilityServices)
        {
            Debug.WriteLine(utilityService.Id);
            Debug.WriteLine(utilityService.IsAllowToRent);
            workspaceType.WorkspaceUtilityServices.FirstOrDefault(x => x.Id == utilityService.Id).IsAllowToRent = utilityService.IsAllowToRent;
        }

        foreach (var workspaceTypeAtBranch in request.WorkspacesAtBranches)
        {
            workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == workspaceTypeAtBranch.BranchId).IsAvailable = workspaceTypeAtBranch.IsAvailable;
            foreach (var workspace in workspaceTypeAtBranch.Workspaces)
            {
                workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == workspaceTypeAtBranch.BranchId).Workspaces
                    .FirstOrDefault(x => x.Id == workspace.Id).IsActive = workspace.IsActive;
            }
        }
        for (var i = 0; i < request.WorkspaceInWorkspaceTypes.Length; i++)
        {
            int currentWorkspaceAmout = workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == request.WorkspaceInWorkspaceTypes[i].BranchId).Workspaces.Count;

            if (workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == request.WorkspaceInWorkspaceTypes[i].BranchId).Workspaces.Count <= 0 && request.WorkspaceInWorkspaceTypes[i].NumberOfWorkspace <= 0)
            {
                workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == request.WorkspaceInWorkspaceTypes[i].BranchId).IsAvailable = false;
            }
            if (request.WorkspaceInWorkspaceTypes[i].NumberOfWorkspace > 0)
            {
                Debug.WriteLine(request.WorkspaceInWorkspaceTypes[i].NumberOfWorkspace);
                for (int j = 0; j < request.WorkspaceInWorkspaceTypes[i].NumberOfWorkspace; j++)
                {
                    var workspace = new Workspace()
                    {
                        WorkspaceNumber = currentWorkspaceAmout + j + 1,
                        IsActive = true,
                        WorkspaceTypeAtBranchId = workspaceType.WorkspacesAtBranches.ToList()
                        .Find(x => x.WorkspaceTypeId.Equals(workspaceType.Id)
                        && x.BranchId.Equals(request.WorkspaceInWorkspaceTypes[i].BranchId)).Id
                    };
                    workspaceType.WorkspacesAtBranches.FirstOrDefault(x => x.BranchId == request.WorkspaceInWorkspaceTypes[i].BranchId).Workspaces.Add(workspace);
                }

            }
        }

        if (request.NewImages.Any())
        {
            foreach (var newImage in request.UpdateWorkspaceTypeImages)
            {
                await _azureService.UploadFile(request.NewImages.FirstOrDefault(x => x.FileName.Equals(newImage.newImageName)), newImage.name);
            }
        }


        var modelUrl = "";
        Debug.WriteLine(request.ModelFile);
        if (request.ModelFile != null)
        {
            modelUrl = await _azureService.UploadModel(request.ModelFile);
        }

        _context.WorkspaceTypes.Update(workspaceType);
        await _context.SaveChangesAsync(cancellationToken);
        var res = await _context.WorkspaceTypes
            .Include(x => x.WorkspaceUtilityServices)
                .ThenInclude(y => y.UtilityService)
            .Include(x => x.WorkspacesAtBranches)
                .ThenInclude(y => y.Branch)
            .Include(x => x.WorkspacesAtBranches)
                .ThenInclude(y => y.Workspaces)
            .Include(x => x.WorkspaceMedias)
            .FirstOrDefaultAsync(x => x.IsActive && x.Id == request.Id, cancellationToken);

        return _mapper.Map<WorkspaceTypeDto>(res);
    }
}