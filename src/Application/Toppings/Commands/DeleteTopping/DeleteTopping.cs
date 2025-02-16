using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Toppings.Commands.DeleteTopping;

public record DeleteToppingCommand : IRequest<ToppingDto>
{
    public Guid Id { get; init; }
}

public class DeleteToppingCommandHandler : IRequestHandler<DeleteToppingCommand, ToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ToppingDto> Handle(DeleteToppingCommand request, CancellationToken cancellationToken)
    {
        var topping = await _context.Toppings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (topping is null)
        {
            throw new KeyNotFoundException($"Topping with id {request.Id} not found");
        }
        
        topping.IsActive = false;
        
        _context.Toppings.Update(topping);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ToppingDto>(topping);
    }
}