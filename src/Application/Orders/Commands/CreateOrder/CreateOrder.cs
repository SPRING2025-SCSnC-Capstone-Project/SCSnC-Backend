using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.Orders.Common;
using Domain.Entities;
using NodaTime;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<ResponseOrderDto>
{
    public Guid? TableId { get; init; }
    public Guid? WorkspaceId { get; init; }
    public Guid UserId { get; init; }
    public Guid? VoucherId { get; init; }
    public List<CreateOrderDetailDto> OrderDetails { get; init; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResponseOrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        #region Manual Validation (temporary)
        //double totalPrice = 0;
        // var checkOrder = _context.Orders.OrderByDescending(x => x.LastUpdatedAt).FirstOrDefaultAsync(x => x.TableId == request.TableId, cancellationToken).Result;
        //
        // if (checkOrder != null && checkOrder.PaymentStatus == false)
        // {
        //     throw new ValidationException("Table is already occupied");
        //     //redirect to update order
        //     
        // }
        
        // Upper comment is code for checking if table is already occupied
        
        
        // Will need another way to validate order either have tableId or workspaceId
        // if both are null, throw exception
        if ((request.TableId.HasValue == false) && (request.WorkspaceId.HasValue == false))
        {
            throw new ValidationException("TableId or WorkspaceId must be provided");
        }
        // if both are not null, throw exception
        if ((request.TableId.HasValue == false) && (request.WorkspaceId.HasValue == false))
        {
            throw new ValidationException("TableId or WorkspaceId must be provided");
        }
        
        #endregion
        
        // start creating order from here
        
        var order = new Order
        {
            TableId = request.TableId,
            WorkspaceId = request.WorkspaceId,
            UserId = request.UserId,
            VoucherId = request.VoucherId,
            //TotalPrice = totalPrice,
            TotalPrice = 0,
            
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            PaymentStatus = false
        };

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

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

            newOrderDetail.TotalPrice = orderDetailPrice * orderDetail.Quantity;
            _context.OrderDetails.Update(newOrderDetail);
            await _context.SaveChangesAsync(cancellationToken);
        }
        var discount = await _context.UserVouchers.Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Id == request.VoucherId, cancellationToken);
        
        if (discount != null)
        {
            if (discount.RedeemStatus == true || discount.Voucher.ExpiredDate < LocalDateTime.FromDateTime(DateTime.Now))
            {
                order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result;
            }
            order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result / 100 * (100 - discount.Voucher.DiscountValue);
        } 
        else order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result;
        
        _context.Orders.Update(order);
        
        await _context.SaveChangesAsync(cancellationToken);

        //var result = _mapper.Map<ResponseOrderDto>(order);
        
        var get = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.User)
            .Include(o => o.Voucher)
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        var result = _mapper.Map<ResponseOrderDto>(get);
        
        result.OrderDetails = _context.OrderDetails
            .Include(od => od.ItemWithSize)
            .Include(od => od.ItemWithSize.Item)
            .Include(od => od.ItemWithSize.Size)
            .Include(od => od.IncludeToppings)
            .ThenInclude(t => t.Topping)
            .Where(od => od.OrderId == order.Id)
            .Select(od => _mapper.Map<OrderDetailDto>(od))
            .ToList();
        
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