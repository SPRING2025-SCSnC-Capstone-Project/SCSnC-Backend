using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.IncludeToppings.Commands.CreateIncludeTopping;

public record CreateIncludeToppingCommand : IRequest<IncludeToppingDto>
{
    public Guid ToppingId { get; init; }
    public Guid OrderDetailId { get; init; }
}

public class CreateIncludeToppingCommandHandler : IRequestHandler<CreateIncludeToppingCommand, IncludeToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateIncludeToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IncludeToppingDto> Handle(CreateIncludeToppingCommand request, CancellationToken cancellationToken)
    {
        var includeTopping = new IncludeTopping
        {
            ToppingId = request.ToppingId,
            OrderDetailId = request.OrderDetailId,
        };
        
        var result = await _context.IncludeToppings.AddAsync(includeTopping, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<IncludeToppingDto>(result.Entity);
    }
}