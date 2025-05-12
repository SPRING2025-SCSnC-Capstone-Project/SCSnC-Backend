using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.WorkspaceTypes.Queries;

public record GetWorkspaceTypeByIdQuery : IRequest<WorkspaceTypeDto> {
    public Guid Id { get; init; }
}

public class GetWorkspaceTypeByIdQueryHandler : IRequestHandler<GetWorkspaceTypeByIdQuery, WorkspaceTypeDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspaceTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkspaceTypeDto> Handle(GetWorkspaceTypeByIdQuery request, CancellationToken cancellationToken) {
        var workspaceType = await _context.WorkspaceTypes.Include(x => x.WorkspaceMedias).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (workspaceType is null) {
            throw new KeyNotFoundException($"WorkspaceType with Id {request.Id} does not exist");
        }

        return _mapper.Map<WorkspaceTypeDto>(workspaceType);
    }
}