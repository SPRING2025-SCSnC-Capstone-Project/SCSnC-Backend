using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Common.Helpers;

public static class PaymentHelper
{
    public static async Task UpdateStatus(string entityId, string switcher, IApplicationDbContext _context, CancellationToken cancellationToken)
    {
        switch (switcher)
        {
            case "Order":
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == Guid.Parse(entityId), cancellationToken);
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with order id {entityId} not found");
                }
        
                order.PaymentStatus = true;
                order.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Orders.Update(order);
                
                var orderTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.OrderId == Guid.Parse(entityId), cancellationToken);
                orderTransaction.TransactionStatus = "Success";
                orderTransaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Transactions.Update(orderTransaction);
                await _context.SaveChangesAsync(cancellationToken);
                
                break;
            case "Reservation":
                var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == Guid.Parse(entityId), cancellationToken);
                if (reservation == null)
                {
                    throw new KeyNotFoundException($"Reservation with reservation id {entityId} not found");
                }
                
                reservation.IsFullPaid = true;
                _context.Reservations.Update(reservation);
                
                var reservationTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.ReservationId == Guid.Parse(entityId));
                reservationTransaction.TransactionStatus = "Success";
                reservationTransaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Transactions.Update(reservationTransaction);
                await _context.SaveChangesAsync(cancellationToken);
                break;
        }
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
            bool checkOrder = orderId.HasValue && await _context.Orders.AnyAsync(x => x.Id == orderId, cancellationToken);
            bool checkReservation = reservationId.HasValue && await _context.Reservations.AnyAsync(x => x.Id == reservationId, cancellationToken);

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
