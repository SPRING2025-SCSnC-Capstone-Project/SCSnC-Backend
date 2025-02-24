using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.ItemCategories.Queries.GetItemCategoriesPaginated;

public record GetItemCategoriesPaginatedQuery : IRequest<PaginatedList<ItemCategoryDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetItemCategoriesPaginatedQueryHandler : IRequestHandler<GetItemCategoriesPaginatedQuery, PaginatedList<ItemCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetItemCategoriesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ItemCategoryDto>> Handle(GetItemCategoriesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ItemCategories.Where(i => i.IsActive == true).AsQueryable();
        
        return await query.ListPaginateWithSortAsync<ItemCategory, ItemCategoryDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}