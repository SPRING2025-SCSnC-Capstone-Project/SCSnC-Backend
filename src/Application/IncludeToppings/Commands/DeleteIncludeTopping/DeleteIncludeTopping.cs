using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.IncludeToppings.Commands.DeleteIncludeTopping;

public record DeleteIncludeToppingCommand : IRequest<IncludeToppingDto>
{
    public Guid Id { get; init; }
}

public class DeleteIncludeToppingCommandHandler : IRequestHandler<DeleteIncludeToppingCommand, IncludeToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteIncludeToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IncludeToppingDto> Handle(DeleteIncludeToppingCommand request, CancellationToken cancellationToken)
    {
        var includeTopping = await _context.IncludeToppings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (includeTopping is null)
        {
            throw new KeyNotFoundException($"IncludeTopping with id {request.Id} not found");
        }
        
        _context.IncludeToppings.Remove(includeTopping);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<IncludeToppingDto>(includeTopping);
    }
}