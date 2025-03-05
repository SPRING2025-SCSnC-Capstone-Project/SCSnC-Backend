using Application.Common.Interfaces;
using NodaTime;

namespace Application.Common.Helpers;

public static class PaymentHelper
{
    public static void UpdateStatus(string orderId, IApplicationDbContext _context, CancellationToken cancellationToken)
    {
        var order = _context.Orders.FirstOrDefault(x => x.Id == Guid.Parse(orderId));
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with order id {orderId} not found");
        }
        
        order.PaymentStatus = true;
        _context.Orders.Update(order);
        _context.SaveChangesAsync(cancellationToken);
        
        var transaction = _context.Transactions.FirstOrDefault(x => x.OrderId == Guid.Parse(orderId));
        transaction.TransactionStatus = "Success";
        transaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Transactions.Update(transaction);
        _context.SaveChangesAsync(cancellationToken);
    }
}