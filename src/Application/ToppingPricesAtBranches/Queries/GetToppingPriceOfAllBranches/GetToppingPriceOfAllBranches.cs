using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ToppingPricesAtBranches.Queries.GetToppingPriceOfAllBranches;

public record GetToppingPriceOfAllBranchesQuery : IRequest<List<ToppingPriceAtBranchDto>>
{
    public Guid ToppingId { get; set; }
}

public class GetToppingPriceOfAllBranchesqueryHandler : IRequestHandler<GetToppingPriceOfAllBranchesQuery, List<ToppingPriceAtBranchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetToppingPriceOfAllBranchesqueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<ToppingPriceAtBranchDto>> Handle(GetToppingPriceOfAllBranchesQuery request, CancellationToken cancellationToken)
    {
        var toppingPrices = await _context.ToppingPricesAtBranches
            .Include(x => x.Branch)
            .Include(x => x.Topping)
            .Where(x => x.ToppingId == request.ToppingId)
            .Select(x => _mapper.Map<ToppingPriceAtBranchDto>(x))
            .ToListAsync(cancellationToken);

        if (toppingPrices is null || toppingPrices.Count == 0)
        {
            throw new KeyNotFoundException($"Topping prices for topping id {request.ToppingId} not found");
        }
        
        return toppingPrices;
    }
}