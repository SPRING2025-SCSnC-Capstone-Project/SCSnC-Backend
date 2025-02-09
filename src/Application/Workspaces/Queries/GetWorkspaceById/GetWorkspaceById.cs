using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Workspaces.Queries;

public record GetWorkspaceByIdQuery : IRequest<WorkspaceDto> {
    public Guid Id { get; init; }
}

public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspaceByIdQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.Include(x => x.WorkspaceType).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.Id} does not exist");
        }

        return _mapper.Map<WorkspaceDto>(workspace);
    }
}