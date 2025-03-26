using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using System.Diagnostics;

namespace Application.Toppings.Commands.AddTopping;

public record AddToppingCommand : IRequest<ToppingDto>
{
    public string ToppingName { get; init; }
    public string ToppingDescription { get; init; }
    public double Price { get; init; }
    public string[]? Toppings { get; init; } = [];
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
        try
        {
            dynamic result = "";
            if (request.Toppings?.Length > 0 && _context.Toppings.ToList().Count <= 0)
            {
                for (int i = 0; i < request.Toppings.Length; i++)
                {
                    var topping = new Topping
                    {
                        ToppingName = request.Toppings[i].Split(":")[0],
                        ToppingDescription = request.Toppings[i].Split(":")[0],
                        Price = double.Parse(request.Toppings[i].Split(":")[1]),
                        IsActive = true,
                        CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                        LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    };
                    result = await _context.Toppings.AddAsync(topping, cancellationToken);
                }
            }
            else
            {
                var topping = new Topping
                {
                    ToppingName = request.ToppingName,
                    ToppingDescription = request.ToppingDescription,
                    Price = request.Price,
                    IsActive = true,
                    CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                    LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                };
                result = await _context.Toppings.AddAsync(topping, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ToppingDto>(result.Entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.InnerException.Message);
        }
    }
}