using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ItemPricesAtBranches.Queries.GetItemPriceOfAllBranches;

public record GetItemPriceOfAllBranchesQuery : IRequest<ItemPriceAtAllBranchesDto>
{
    public Guid ItemId { get; init; }
}

public class GetItemPriceOfAllBranchesQueryHandler : IRequestHandler<GetItemPriceOfAllBranchesQuery, ItemPriceAtAllBranchesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetItemPriceOfAllBranchesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ItemPriceAtAllBranchesDto> Handle(GetItemPriceOfAllBranchesQuery request, CancellationToken cancellationToken)
    {
        var itemPrices = await _context.ItemPricesAtBranches
            .Include(x => x.Branch)
            .Include(x => x.Item)
            .Where(x => x.ItemId == request.ItemId)
            .ToListAsync(cancellationToken);
        
        if (itemPrices is null || itemPrices.Count == 0)
        {
            throw new KeyNotFoundException($"Item prices for item with id {request.ItemId} not found");
        }
        
        return _mapper.Map<ItemPriceAtAllBranchesDto>(itemPrices);
    }
}