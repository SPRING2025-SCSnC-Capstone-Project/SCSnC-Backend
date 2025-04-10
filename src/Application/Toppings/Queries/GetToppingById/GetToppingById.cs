using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Toppings.Queries.GetToppingById;

public record GetToppingByIdQuery : IRequest<ToppingDto>
{
    public Guid Id { get; init; }
    public Guid BranchId { get; init; }
}

public class GetToppingByIdQueryHandler : IRequestHandler<GetToppingByIdQuery, ToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetToppingByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ToppingDto> Handle(GetToppingByIdQuery request, CancellationToken cancellationToken)
    {
        var topping = await _context.Toppings
            .Include(x => x.ToppingPricesAtBranches.Where(y => y.BranchId == request.BranchId))
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (topping is null)
        {
            throw new KeyNotFoundException($"Topping with id {request.Id} not found");
        }
        
        return _mapper.Map<ToppingDto>(topping);
    }
}