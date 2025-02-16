using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.IncludeToppings.Commands.UpdateIncludeTopping;

public record UpdateIncludeToppingCommand : IRequest<IncludeToppingDto>
{
    public Guid Id { get; init; }
    public Guid ToppingId { get; init; }
    public Guid OrderDetailId { get; init; }
}

public class UpdateIncludeToppingCommandHandler : IRequestHandler<UpdateIncludeToppingCommand, IncludeToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateIncludeToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IncludeToppingDto> Handle(UpdateIncludeToppingCommand request, CancellationToken cancellationToken)
    {
        var includeTopping = await _context.IncludeToppings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (includeTopping is null)
        {
            throw new KeyNotFoundException($"IncludeTopping with id {request.Id} not found");
        }
        
        includeTopping.ToppingId = request.ToppingId;
        includeTopping.OrderDetailId = request.OrderDetailId;
        
        _context.IncludeToppings.Update(includeTopping);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<IncludeToppingDto>(includeTopping);
    }
}