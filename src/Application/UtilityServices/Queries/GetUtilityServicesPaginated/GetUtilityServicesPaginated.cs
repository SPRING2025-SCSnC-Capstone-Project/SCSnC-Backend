using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.UtilityServices.Queries.GetUtilityServicesPaginated;

public record GetUtilityServicesPaginatedQuery : IRequest<PaginatedList<UtilityServiceDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetUtilityServicesPaginatedQueryHandler: IRequestHandler<GetUtilityServicesPaginatedQuery, PaginatedList<UtilityServiceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUtilityServicesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<UtilityServiceDto>> Handle(GetUtilityServicesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.UtilityServices.AsQueryable();
        
        return await query.ListPaginateWithSortAsync<UtilityService, UtilityServiceDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken);
    }
}