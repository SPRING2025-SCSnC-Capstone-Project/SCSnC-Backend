using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Items.Queries.GetItemsPaginated;

public record GetItemsPaginatedQuery : IRequest<PaginatedList<ItemDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? FilterByCategory { get; init; }
}

public class GetItemsPaginatedQueryHandler : IRequestHandler<GetItemsPaginatedQuery, PaginatedList<ItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ItemDto>> Handle(GetItemsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = string.IsNullOrEmpty(request.FilterByCategory) ?
                _context.Items.Include(x => x.ItemCategory).Include(x => x.ItemWithSizes).ThenInclude(y => y.Size).AsQueryable()
                :
                _context.Items.Where(x => x.ItemCategory.CategoryName == request.FilterByCategory).Include(x => x.ItemCategory).Include(x => x.ItemWithSizes).ThenInclude(y => y.Size).AsQueryable()
                ;

        return await query.ListPaginateWithSortAsync<Item, ItemDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}