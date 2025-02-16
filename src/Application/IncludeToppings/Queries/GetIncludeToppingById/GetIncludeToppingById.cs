using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.IncludeToppings.Queries.GetIncludeToppingById;

public record GetIncludeToppingByIdQuery : IRequest<IncludeToppingDto>
{
    public Guid Id { get; init; }
}

public class GetIncludeToppingByIdQueryHandler : IRequestHandler<GetIncludeToppingByIdQuery, IncludeToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetIncludeToppingByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IncludeToppingDto> Handle(GetIncludeToppingByIdQuery request, CancellationToken cancellationToken)
    {
        var includeTopping = await _context.IncludeToppings
            .Include(x => x.Topping)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (includeTopping is null)
        {
            throw new KeyNotFoundException($"IncludeTopping with id {request.Id} not found");
        }
        
        return _mapper.Map<IncludeToppingDto>(includeTopping);
    }
}