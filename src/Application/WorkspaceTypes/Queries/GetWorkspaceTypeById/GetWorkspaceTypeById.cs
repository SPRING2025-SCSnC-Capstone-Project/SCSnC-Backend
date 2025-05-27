using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using System.Diagnostics;

namespace Application.WorkspaceTypes.Queries;

public record GetWorkspaceTypeByIdQuery : IRequest<WorkspaceTypeDto>
{
    public Guid Id { get; init; }
}

public class GetWorkspaceTypeByIdQueryHandler : IRequestHandler<GetWorkspaceTypeByIdQuery, WorkspaceTypeDto>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspaceTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceTypeDto> Handle(GetWorkspaceTypeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workspaceType = await _context.WorkspaceTypes.Include(x => x.WorkspaceMedias)
                .Include(x => x.WorkspacesAtBranches)
                    .ThenInclude(y => y.Branch)
                .Include(x => x.WorkspacesAtBranches)
                    .ThenInclude(y => y.Workspaces)
                .AsNoTracking()
                .Include(x => x.WorkspaceUtilityServices)
                .ThenInclude(y => y.UtilityService)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

            Debug.WriteLine(workspaceType.WorkspaceUtilityServices.ToList()[0].UtilityService.ServiceName);

            if (workspaceType is null)
            {
                throw new KeyNotFoundException($"WorkspaceType with Id {request.Id} does not exist");
            }

            return _mapper.Map<WorkspaceTypeDto>(workspaceType);
        }

        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message, ex);
        }
    }
}