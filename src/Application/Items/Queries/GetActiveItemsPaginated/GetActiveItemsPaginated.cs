using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Items.Queries.GetActiveItemsPaginated;

public record GetActiveItemsPaginatedQuery : IRequest<PaginatedList<ItemDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? FilterByCategory { get; init; }
    public Guid BranchId { get; init; }
}

public class GetActiveItemsPaginatedQueryHandler : IRequestHandler<GetActiveItemsPaginatedQuery, PaginatedList<ItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActiveItemsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ItemDto>> Handle(GetActiveItemsPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = string.IsNullOrEmpty(request.FilterByCategory) ?
                _context.Items
                .Include(x => x.ItemCategory)
                .Include(x => x.ItemWithSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ItemPricesAtBranches.Where(y => y.BranchId == request.BranchId))
                .ThenInclude(y => y.Branch)
                .AsQueryable() 
                :
                _context.Items
                .Include(x => x.ItemCategory)
                .Include(x => x.ItemWithSizes)
                .ThenInclude(x => x.Size)
                .Include(x => x.ItemPricesAtBranches.Where(y => y.BranchId == request.BranchId))
                .ThenInclude(y => y.Branch)
                .Where(x => x.ItemCategory.CategoryName.ToLower().Contains(request.FilterByCategory.ToLower()))
                .AsQueryable();

            return await query.ListPaginateWithSortAsync<Item, ItemDto>(
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
            Debug.WriteLine( ex );
            throw new Exception(ex.Message);
        }
    }
}