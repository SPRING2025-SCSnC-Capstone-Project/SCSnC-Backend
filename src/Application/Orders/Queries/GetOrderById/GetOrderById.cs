using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery : IRequest<ResponseOrderDto>
{
    public Guid OrderId { get; init; }
}

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ResponseOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResponseOrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.User)
            .Include(o => o.Voucher)
            // .ThenInclude(o => o.ItemWithSizes)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with order id {request.OrderId} not found");
        }

        var response = _mapper.Map<ResponseOrderDto>(order);
        
        response.OrderDetails = _context.OrderDetails
            .Include(od => od.ItemWithSize)
            .Include(od => od.ItemWithSize.Item)
            .Include(od => od.ItemWithSize.Size)
            .Include(od => od.IncludeToppings)
            .ThenInclude(t => t.Topping)
            .Where(od => od.OrderId == order.Id)
            .Select(od => _mapper.Map<OrderDetailDto>(od))
            .ToList();
        
        return response;
    }
}
