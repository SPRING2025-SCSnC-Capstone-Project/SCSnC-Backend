using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using System.Diagnostics;

namespace Application.Workspaces.Commands;



public record AddWorkspaceCommand : IRequest<WorkspaceDto>
{
    public int WorkspaceNumber { get; init; }
    public Guid WorkspaceTypeId { get; init; }
    public List<string> MediaTypes { get; init; } = null!;
    public List<string> MediaUrls { get; init; } = null!;
}

public class AddWorkspaceCommandHandler : IRequestHandler<AddWorkspaceCommand, WorkspaceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddWorkspaceCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(AddWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.WorkspaceNumber == request.WorkspaceNumber && x.IsActive, cancellationToken);

        if (workspace is not null)
        {
            throw new ConflictException($"Workspace with number {request.WorkspaceNumber} already exists");
        }

        var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.Id == request.WorkspaceTypeId && x.IsActive, cancellationToken);

        if (workspaceType is null)
        {
            throw new KeyNotFoundException($"Workspace type with Id {request.WorkspaceTypeId} does not exists");
        }

        string[] workspaces = "phòng họp:l:200000,phòng cặp đôi:s:50000,phòng trà:m:100000,phòng đơn:xs:30000".Split(',');
        List<WorkspaceType> workspaceTypes = _context.WorkspaceTypes.ToList();
        List<Branch> branches = _context.Branches.ToList();
        List<WorkspaceTypeAtBranch> workspaceTypeAtBranches = new List<WorkspaceTypeAtBranch>();
        dynamic result = "";

        var addedWorkspace = new Workspace();

        Debug.WriteLine(_context.Workspaces.ToList().Count);

        if (_context.Workspaces.ToList().Count <= 0)
        {
            //for (int i = 0; i < workspaces.Length; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        var entity = new Workspace()
            //        {
            //            WorkspaceNumber = j + 1,
            //            IsAvailable = true,
            //            IsActive = true,
            //            PricePerHour = double.Parse(workspaces[i].Split(":")[2]),
            //            //WorkspaceImageUrl = request.WorkspaceImageUrl,
            //            WorkspaceTypeId = workspaceTypes.FirstOrDefault(x => x.WorkspaceTypeName.Equals(workspaces[i].Split(":")[1])).Id,
            //        };
            //        result = await _context.Workspaces.AddAsync(entity, cancellationToken);
            //        addedWorkspace = await _context.Workspaces
            //            .Include(x => x.WorkspaceMedias)
            //            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
            //    }
            //}

            foreach (var branch in branches)
            {
                foreach (var wt in workspaceTypes)
                {
                    var entity = new WorkspaceTypeAtBranch()
                    {
                        BranchId = branch.Id,
                        WorkspaceTypeId = wt.Id,
                        PriceAdjust = wt.PricePerHour - 0,
                        CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                        LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    result = await _context.WorkspaceTypeAtBranches.AddAsync(entity, cancellationToken);
                    workspaceTypeAtBranches.Add(result.Entity);
                }
            }
            foreach (var workspaceTypeAtBranch in workspaceTypeAtBranches)
            {
                for(int i = 0; i < 10; i++)
                {
                    var entity = new Workspace()
                    {
                        WorkspaceNumber = i + 1,
                        IsAvailable = true,
                        IsActive = true,
                        PricePerHour = 0,
                        //WorkspaceImageUrl = request.WorkspaceImageUrl,
                        WorkspaceTypeAtBranchId = workspaceTypeAtBranch.Id,
                    };
                    await _context.Workspaces.AddAsync(entity, cancellationToken);
                }

            }
        }
        /*else
        {
            foreach (var mediaType in request.MediaTypes)
            {
                if (!mediaType.Trim().ToLower().Equals("3d model")
                        && !mediaType.Trim().ToLower().Equals("image"))
                {
                    throw new InvalidDataException($"Invalid media type: {mediaType}");
                }
            }

            var entity = new Workspace()
            {
                WorkspaceNumber = request.WorkspaceNumber,
                IsAvailable = true,
                IsActive = true,
                //WorkspaceImageUrl = request.WorkspaceImageUrl,
                WorkspaceTypeId = request.WorkspaceTypeId,
            };

            result = await _context.Workspaces.AddAsync(entity, cancellationToken);
            var workspaceMediasToAdd = new List<WorkspaceMedia>();

            for (var i = 0; i < request.MediaTypes.Count; i++)
            {
                var workspaceMedia = new WorkspaceMedia()
                {
                    WorkspaceId = result.Entity.Id,
                    MediaType = request.MediaTypes.ElementAt(i),
                    MediaUrl = request.MediaUrls.ElementAt(i),
                };

                workspaceMediasToAdd.Add(workspaceMedia);
            }
            await _context.WorkspaceMedias.AddRangeAsync(workspaceMediasToAdd, cancellationToken);
            addedWorkspace = await _context.Workspaces
                .Include(x => x.WorkspaceMedias)
                .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
        }*/

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<WorkspaceDto>(addedWorkspace);
    }
}
