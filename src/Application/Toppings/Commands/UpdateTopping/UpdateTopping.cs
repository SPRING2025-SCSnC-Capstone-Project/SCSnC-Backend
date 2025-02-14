using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Toppings.Commands.UpdateTopping;

public record UpdateToppingCommand : IRequest<ToppingDto>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public double Price { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateToppingCommandHandler : IRequestHandler<UpdateToppingCommand, ToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ToppingDto> Handle(UpdateToppingCommand request, CancellationToken cancellationToken)
    {
        var topping = await _context.Toppings.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (topping is null)
        {
            throw new KeyNotFoundException($"Topping with id {request.Id} not found");
        }
        
        topping.ToppingName = request.Name;
        topping.ToppingDescription = request.Description;
        topping.Price = request.Price;
        topping.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        topping.IsActive = request.IsActive;
        
        _context.Toppings.Update(topping);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ToppingDto>(topping);
    }
}