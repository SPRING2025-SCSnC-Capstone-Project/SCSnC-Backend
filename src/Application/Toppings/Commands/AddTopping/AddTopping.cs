using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Toppings.Commands.AddTopping;

public record AddToppingCommand : IRequest<ToppingDto>
{
    public string ToppingName { get; init; }
    public string ToppingDescription { get; init; }
    public double Price { get; init; }
}

public class AddToppingCommandHandler : IRequestHandler<AddToppingCommand, ToppingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public AddToppingCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ToppingDto> Handle(AddToppingCommand request, CancellationToken cancellationToken)
    {
        var topping = new Topping
        {
            ToppingName = request.ToppingName,
            ToppingDescription = request.ToppingDescription,
            Price = request.Price,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
        };
        
        var result = await _context.Toppings.AddAsync(topping, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ToppingDto>(result.Entity);
    }
}