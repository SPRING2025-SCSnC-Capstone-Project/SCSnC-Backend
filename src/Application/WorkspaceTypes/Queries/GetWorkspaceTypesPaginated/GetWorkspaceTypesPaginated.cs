using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.WorkspaceTypes.Queries;

public record GetWorkspaceTypesPaginatedQuery: IRequest<PaginatedList<WorkspaceTypeDto>> {
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetWorkspaceTypesPaginatedQueryHandler: IRequestHandler<GetWorkspaceTypesPaginatedQuery, PaginatedList<WorkspaceTypeDto>> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspaceTypesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkspaceTypeDto>> Handle(GetWorkspaceTypesPaginatedQuery request, CancellationToken cancellationToken) {
        var workspaceTypes = _context.WorkspaceTypes.Include(x => x.WorkspaceMedias).AsQueryable().Where(x => x.IsActive);

        return await workspaceTypes.ListPaginateWithSortAsync<WorkspaceType, WorkspaceTypeDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}