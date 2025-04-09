using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ToppingPricesAtBranches.Queries.GetToppingPriceOfAllBranches;

public record GetToppingPriceOfAllBranchesQuery : IRequest<ToppingPriceAtAllBranchesDto>
{
    public Guid ToppingId { get; set; }
}

public class GetToppingPriceOfAllBranchesqueryHandler : IRequestHandler<GetToppingPriceOfAllBranchesQuery, ToppingPriceAtAllBranchesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetToppingPriceOfAllBranchesqueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ToppingPriceAtAllBranchesDto> Handle(GetToppingPriceOfAllBranchesQuery request, CancellationToken cancellationToken)
    {
        var toppingPrices = await _context.ToppingPricesAtBranches
            .Include(x => x.Branch)
            .Include(x => x.Topping)
            .Where(x => x.ToppingId == request.ToppingId)
            .ToListAsync(cancellationToken);

        if (toppingPrices is null || toppingPrices.Count == 0)
        {
            throw new KeyNotFoundException($"Topping prices for topping id {request.ToppingId} not found");
        }
        
        return _mapper.Map<ToppingPriceAtAllBranchesDto>(toppingPrices);
    }
}