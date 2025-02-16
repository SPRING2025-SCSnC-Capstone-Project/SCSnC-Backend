using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Items.Queries.GetActiveItemsPaginated;

public record GetActiveItemsPaginatedQuery : IRequest<PaginatedList<ItemDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
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
    
    public async Task<PaginatedList<ItemDto>> Handle(GetActiveItemsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Items.Include(x => x.ItemCategory).Where(i => i.IsActive == true).AsQueryable();
        
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