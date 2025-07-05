using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.IncludeToppings.Queries.GetIncludeToppingsByOrderDetailId;

public record GetIncludeToppingsByOrderDetailIdQuery : IRequest<List<IncludeToppingDto>>
{
    public Guid OrderDetailId { get; init; }
}

public class GetIncludeToppingsByOrderDetailIdQueryHandler : IRequestHandler<GetIncludeToppingsByOrderDetailIdQuery, List<IncludeToppingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetIncludeToppingsByOrderDetailIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<IncludeToppingDto>> Handle(GetIncludeToppingsByOrderDetailIdQuery request, CancellationToken cancellationToken)
    {
        var includeToppings = await _context.IncludeToppings
            .Where(x => x.OrderDetailId == request.OrderDetailId)
            .Include(x => x.Topping)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<IncludeToppingDto>>(includeToppings);
    }
}