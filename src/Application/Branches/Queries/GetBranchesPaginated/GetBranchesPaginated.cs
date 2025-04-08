using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Branches.Queries.GetBranchesPaginated;

public record GetBranchesPaginatedQuery : IRequest<PaginatedList<BranchDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetBranchesPaginatedQueryHandler : IRequestHandler<GetBranchesPaginatedQuery, PaginatedList<BranchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetBranchesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<BranchDto>> Handle(GetBranchesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Branches.Where(x => x.IsActive == true).AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Branch, BranchDto>
        (
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}