using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.Orders.Common;
using Domain.Entities;

namespace Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand : IRequest<OrderDto>
{
    public Guid OrderId { get; init; }
    public List<CreateOrderDetailDto> OrderDetails { get; init; }
}

/*
 * Curently, the UpdateOrder class only updates the OrderDetails of an Order.
 * Will need to discuss with mentor on whether to allow changing from current item to another item after created an order.
 * If allowed, the previous OrderDetails will need to be deleted and new OrderDetails will be created.
 * Users will need to reselect the items and toppings.
 */

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
        
        if (order is null)
        {
            throw new KeyNotFoundException($"Order with id {request.OrderId} not found");
        }
        
        if (order.PaymentStatus)
        {
            throw new ValidationException("Order has been paid, cannot be updated");
        }

        foreach (var orderDetail in request.OrderDetails)
        {
            double orderDetailPrice = 0;
            
            var newOrderDetail = new OrderDetail
            {
                ItemWithSizeId = _context.ItemWithSizes.FirstOrDefaultAsync(x =>
                    x.ItemId == orderDetail.ItemId && x.SizeId == orderDetail.SizeId, cancellationToken).Result.Id,
                OrderId = order.Id,
                Quantity = orderDetail.Quantity,
                TotalPrice = orderDetailPrice
            };
            
            await _context.OrderDetails.AddAsync(newOrderDetail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == orderDetail.ItemId, cancellationToken);
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == orderDetail.SizeId, cancellationToken);
            
            if (item is null)
            {
                throw new KeyNotFoundException($"Item with id {orderDetail.ItemId} not found");
            }
            
            orderDetailPrice += item.ItemBasePrice + size.PriceAdjustment;
            //totalPrice += item.ItemBasePrice + size.PriceAdjustment;

            foreach (var includeTopping in orderDetail.ToppingIds)
            {
                var topping =
                    await _context.Toppings.FirstOrDefaultAsync(x => x.Id == includeTopping, cancellationToken);
                if (topping is null)
                {
                    throw new KeyNotFoundException($"IncludeTopping with id {topping} not found");
                }
                orderDetailPrice += topping.Price;
                //totalPrice += topping.Price;

                var newincludeTopping = new IncludeTopping
                {
                    OrderDetailId = newOrderDetail.Id,
                    ToppingId = topping.Id
                };
                
                await _context.IncludeToppings.AddAsync(newincludeTopping, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        
        var discount = await _context.Vouchers.FirstOrDefaultAsync(x => x.Id == order.VoucherId, cancellationToken);
        
        if (discount == null) order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result;
        else order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result / 100 * (100 - discount.DiscountValue);

        
        _context.Orders.Update(order);
        
        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<OrderDto>(order);
        
        //include missing data in return result
        result.TableNumber = _context.Tables.FirstOrDefaultAsync(x => x.Id == order.TableId, cancellationToken).Result.TableNumber;
        
        return result;
    }
    
    // private async Task<double> CalculateTotalPrice(Guid orderId, CancellationToken cancellationToken)
    // {
    //     var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync(cancellationToken);
    //     double totalPrice = 0;
    //     
    //     foreach (var orderDetail in orderDetails)
    //     {
    //         var itemWithSize = await _context.ItemWithSizes.FirstOrDefaultAsync(x => x.Id == orderDetail.ItemWithSizeId, cancellationToken);
    //         var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == itemWithSize.ItemId, cancellationToken);
    //         var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == itemWithSize.SizeId, cancellationToken);
    //         
    //         totalPrice += item.ItemBasePrice + size.PriceAdjustment;
    //         
    //         var includeToppings = await _context.IncludeToppings.Where(x => x.OrderDetailId == orderDetail.Id).ToListAsync(cancellationToken);
    //         foreach (var includeTopping in includeToppings)
    //         {
    //             var topping = await _context.Toppings.FirstOrDefaultAsync(x => x.Id == includeTopping.ToppingId, cancellationToken);
    //             totalPrice += topping.Price;
    //         }
    //     }
    //     
    //     return totalPrice;
    // }
}