using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Workspaces.Queries;

public record GetWorkspacesPaginatedQuery : IRequest<PaginatedList<WorkspaceDto>>
{
    public string? Filter { get; set; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }

}

public class GetWorkspacesPaginatedQueryHandler : IRequestHandler<GetWorkspacesPaginatedQuery, PaginatedList<WorkspaceDto>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspacesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkspaceDto>> Handle(GetWorkspacesPaginatedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workspaces = _context.Workspaces.Include(x => x.WorkspaceType)
                .Where(x => x.IsActive)
                .AsQueryable();

            if (request.Filter != null && !request.Filter.Equals(string.Empty))
            {
                workspaces = workspaces.Where(x => x.WorkspaceType.WorkspaceTypeName == request.Filter);
            }

            return await workspaces.ListPaginateWithSortAsync<Workspace, WorkspaceDto>(
                request.Page,
                request.Size,
                request.SortBy,
                request.SortOrder,
                _mapper.ConfigurationProvider,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message);
        }

    }
}