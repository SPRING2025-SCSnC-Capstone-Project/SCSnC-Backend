using Application.Common.Interfaces;

namespace Application.Orders.Common;

public static class OrderHelper
{
    public static async Task<double> CalculateTotalPrice(Guid orderId, CancellationToken cancellationToken, IApplicationDbContext _context)
    {
        var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync(cancellationToken);
        double totalPrice = 0;
        
        foreach (var orderDetail in orderDetails)
        {
            var itemWithSize = await _context.ItemWithSizes.FirstOrDefaultAsync(x => x.Id == orderDetail.ItemWithSizeId, cancellationToken);
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == itemWithSize.ItemId, cancellationToken);
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == itemWithSize.SizeId, cancellationToken);
            
            double toppingPrice = 0;
            
            var includeToppings = await _context.IncludeToppings.Where(x => x.OrderDetailId == orderDetail.Id).ToListAsync(cancellationToken);
            foreach (var includeTopping in includeToppings)
            {
                var topping = await _context.Toppings.FirstOrDefaultAsync(x => x.Id == includeTopping.ToppingId, cancellationToken);
                toppingPrice += topping.Price;
            }
            totalPrice += (item.ItemBasePrice + size.PriceAdjustment + toppingPrice) * orderDetail.Quantity;
        }

        return totalPrice;
    }
}