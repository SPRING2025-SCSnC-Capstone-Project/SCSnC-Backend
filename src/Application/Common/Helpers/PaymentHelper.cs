using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
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
        order.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        _context.Orders.Update(order);
        _context.SaveChangesAsync(cancellationToken);
        
        var transaction = _context.Transactions.FirstOrDefault(x => x.OrderId == Guid.Parse(orderId));
        transaction.TransactionStatus = "Success";
        transaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Transactions.Update(transaction);
        _context.SaveChangesAsync(cancellationToken);
    }

    public static async Task<TransactionCreateStatus> CreateTransaction(
        Guid? orderId, 
        Guid? reservationId, 
        double amount, string paymentMethod, 
        IApplicationDbContext _context, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = new TransactionCreateStatus();
            bool checkOrder = orderId.HasValue && _context.Orders.Any(x => x.Id == orderId);
            bool checkReservation = reservationId.HasValue && _context.Reservations.Any(x => x.Id == reservationId);

            switch (checkOrder, checkReservation)
            {
                case (true, false):
                    
                    var orderTransaction = new Transaction
                    {
                        OrderId = orderId,
                        ReservationId = null,
                        Amount = amount,
                        PaymentMethod = paymentMethod,
                        TransactionStatus = "Pending",
                        TransactionDate = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    
                    await _context.Transactions.AddAsync(orderTransaction, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    result.Message = "Success";
                    result.IsSuccess = true;
                    
                    break;
                case (false, true):
                    
                    var reservationTransaction = new Transaction
                    {
                        OrderId = null,
                        ReservationId = reservationId,
                        Amount = amount,
                        PaymentMethod = paymentMethod,
                        TransactionStatus = "Pending",
                        TransactionDate = LocalDateTime.FromDateTime(DateTime.Now)
                    };
                    
                    await _context.Transactions.AddAsync(reservationTransaction, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    result.Message = "Success";
                    result.IsSuccess = true;
                    
                    break;
                case (false, false):
                    result.Message = "Either Order or Reservation ID must be provided";
                    result.IsSuccess = false;
                    break;
                case (true, true):
                    result.Message = "Both Order and Reservation ID cannot be provided at the same time";
                    result.IsSuccess = false;
                    break;
            }
            
            return result;
        }
        catch (Exception ex)
        {
            return new TransactionCreateStatus
            {
                Message = ex.Message,
                IsSuccess = false
            };
        }
    }
}