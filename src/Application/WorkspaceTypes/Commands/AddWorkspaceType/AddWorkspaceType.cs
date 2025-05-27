using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using NodaTime;
using System.Diagnostics;

namespace Application.WorkspaceTypes.Commands;

public class WorkspaceInWorkspaceType
{
    public Guid BranchId { get; set; }
    public int NumberOfWorkspace { get; set; }
}

public record AddWorkspaceTypeCommand : IRequest<WorkspaceTypeDto>
{
    public string WorkspaceTypeName { get; init; } = null!;
    public string WorkspaceTypeDescription { get; set; } = null!;
    public int MaxCapacity { get; init; }
    public double PricePerHour { get; set; }
    public Guid[] BranchId { get; init; } = null!;
    public IFormFile[] Images { get; init; } = null!;
    public WorkspaceInWorkspaceType[] WorkspaceInWorkspaceTypes { get; init; } = null!;
}

public class AddWorkspaceTypeComamndHandler : IRequestHandler<AddWorkspaceTypeCommand, WorkspaceTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureService _azureService;

    public AddWorkspaceTypeComamndHandler(IApplicationDbContext context, IMapper mapper, IAzureService azureService)
    {
        _context = context;
        _mapper = mapper;
        _azureService = azureService;
    }

    public async Task<WorkspaceTypeDto> Handle(AddWorkspaceTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.IsActive && x.WorkspaceTypeName.Equals(request.WorkspaceTypeName), cancellationToken);
            var availableUtility = _context.UtilityServices.ToList();
            var availableBranch = _context.Branches.ToList();
            var addedWorkspaceType = new WorkspaceType();
            var numberOfImagesRequired = 3;

            Debug.WriteLine(request.Images.Length);

            if (workspaceType is not null)
            {
                throw new ConflictException($"Workspace type with name {request.WorkspaceTypeName} already exists");
            }

            var entity = new WorkspaceType()
            {
                WorkspaceTypeName = request.WorkspaceTypeName,
                Description = request.WorkspaceTypeDescription,
                MaxCapacity = request.MaxCapacity,
                PricePerHour = request.PricePerHour,
                IsActive = true,
            };
            var result = await _context.WorkspaceTypes.AddAsync(entity, cancellationToken);

            var workspaceMediasToAdd = new List<WorkspaceMedia>();
            var workspaceTypeAtBranchToAdd = new List<WorkspaceTypeAtBranch>();
            var workspaceUtilityServiceToAdd = new List<WorkspaceUtilityService>();
            var workspaceToAdd = new List<Workspace>();
            var imageUrl = new List<string>();
            var addToDbImageUrl = new List<string>();

            if (request.Images.Any())
            {
                imageUrl = await _azureService.UploadMultipleImage(request.Images, entity.WorkspaceTypeName);
            }

            for (var i = 0; i < imageUrl.Count; i++)
            {
                var workspaceMedia = new WorkspaceMedia()
                {
                    WorkspaceTypeId = result.Entity.Id,
                    MediaType = "image",
                    MediaUrl = imageUrl[i],
                };
                workspaceMediasToAdd.Add(workspaceMedia);
            }

            Debug.WriteLine(workspaceMediasToAdd.Count);

            for (var i = 0; i < availableBranch.Count; i++)
            {
                var workspaceTypeAtBranch = new WorkspaceTypeAtBranch()
                {
                    BranchId = availableBranch[i].Id,
                    WorkspaceTypeId = result.Entity.Id,
                    CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    IsAvailable = request.BranchId.Contains(availableBranch[i].Id)
                };
                var addWsTABResult = await _context.WorkspaceTypeAtBranches.AddAsync(workspaceTypeAtBranch, cancellationToken);
                workspaceTypeAtBranchToAdd.Add(addWsTABResult.Entity);
            }

            for (int i = 0; i < request.WorkspaceInWorkspaceTypes.Length; i++)
            {
                for (int j = 0; j < request.WorkspaceInWorkspaceTypes[i].NumberOfWorkspace; j++)
                {
                    var workspace = new Workspace()
                    {
                        WorkspaceNumber = j + 1,
                        IsActive = true,
                        WorkspaceTypeAtBranchId = workspaceTypeAtBranchToAdd
                        .Find(x => x.WorkspaceTypeId.Equals(result.Entity.Id)
                        && x.BranchId.Equals(request.WorkspaceInWorkspaceTypes[i].BranchId)).Id,
                        
                    };
                    workspaceToAdd.Add(workspace);
                }

            }

            for (int i = 0; i < availableUtility.Count; i++)
            {
                var workspaceUtilityService = new WorkspaceUtilityService()
                {
                    IsAllowToRent = false,
                    UtilityServiceId = availableUtility[i].Id,
                    WorkspaceTypeId = result.Entity.Id,
                };
                workspaceUtilityServiceToAdd.Add(workspaceUtilityService);
            }

            await _context.WorkspaceMedias.AddRangeAsync(workspaceMediasToAdd, cancellationToken);
            await _context.Workspaces.AddRangeAsync(workspaceToAdd, cancellationToken);
            await _context.WorkspaceUtilityServices.AddRangeAsync(workspaceUtilityServiceToAdd, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var workspaceTypeAtBranches = _context.WorkspaceTypeAtBranches.Include(x => x.Branch).Where(x => x.WorkspaceTypeId.Equals(result.Entity.Id)).ToList();
            var workspaceUtilityServices = _context.WorkspaceUtilityServices.Include(x => x.UtilityService).Where(x => x.WorkspaceTypeId.Equals(result.Entity.Id)).ToList();
            addedWorkspaceType = await _context.WorkspaceTypes.Include(x => x.WorkspaceMedias).Include(x => x.WorkspacesAtBranches).Include(x => x.WorkspaceUtilityServices).FirstOrDefaultAsync(x => x.Id.Equals(result.Entity.Id), cancellationToken);
            addedWorkspaceType.WorkspacesAtBranches = workspaceTypeAtBranches;
            addedWorkspaceType.WorkspaceUtilityServices = workspaceUtilityServices;
            Debug.WriteLine(addedWorkspaceType.WorkspacesAtBranches.Count);
            Debug.WriteLine(workspaceTypeAtBranchToAdd.Count);

            return _mapper.Map<WorkspaceTypeDto>(addedWorkspaceType);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            throw new Exception(ex.Message, ex);
        }

    }
}