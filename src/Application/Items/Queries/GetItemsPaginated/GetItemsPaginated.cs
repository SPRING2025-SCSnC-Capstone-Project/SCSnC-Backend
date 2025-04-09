using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Items.Queries.GetItemsPaginated;

public record GetItemsPaginatedQuery : IRequest<PaginatedList<ItemInfoDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? FilterByCategory { get; init; }
}

public class GetItemsPaginatedQueryHandler : IRequestHandler<GetItemsPaginatedQuery, PaginatedList<ItemInfoDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ItemInfoDto>> Handle(GetItemsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = string.IsNullOrEmpty(request.FilterByCategory)?
            _context.Items
                .Include(x => x.ItemCategory)
                .Include(x => x.ItemWithSizes)
                .ThenInclude(y => y.Size)
                .Include(x => x.ItemPricesAtBranches.Where(y => y.ItemId == x.Id))
                .ThenInclude(br => br.Branch)
                .AsQueryable():
            _context.Items
                .Include(x => x.ItemCategory)
                .Include(x => x.ItemWithSizes)
                .ThenInclude(y => y.Size)
                .Include(x => x.ItemPricesAtBranches.Where(y => y.ItemId == x.Id))
                .ThenInclude(br => br.Branch)
                .Where(x => x.ItemCategory.CategoryName == request.FilterByCategory)
                .AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Item, ItemInfoDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}