using Application.Common.Interfaces;
using NodaTime;

namespace Application.Common.Helpers;

public static class PaymentHelper
{
    public static async Task UpdateStatus(string orderId, IApplicationDbContext _context, CancellationToken cancellationToken)
    {
        var order = _context.Orders.FirstOrDefault(x => x.Id == Guid.Parse(orderId));
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with order id {orderId} not found");
        }
        
        order.PaymentStatus = true;
        order.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        
        var transaction = _context.Transactions.FirstOrDefault(x => x.OrderId == Guid.Parse(orderId));
        transaction.TransactionStatus = "Success";
        transaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }
}