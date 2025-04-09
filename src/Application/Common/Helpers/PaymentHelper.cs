using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Common.Helpers;

public static class PaymentHelper
{
    public static void UpdateStatus(string entityId, string switcher, IApplicationDbContext _context, CancellationToken cancellationToken)
    {
        switch (switcher)
        {
            case "Order":
                var order = _context.Orders.FirstOrDefault(x => x.Id == Guid.Parse(entityId));
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with order id {entityId} not found");
                }
        
                order.PaymentStatus = true;
                order.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Orders.Update(order);
                _context.SaveChangesAsync(cancellationToken);
                
                var orderTransaction = _context.Transactions.FirstOrDefault(x => x.OrderId == Guid.Parse(entityId));
                orderTransaction.TransactionStatus = "Success";
                orderTransaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Transactions.Update(orderTransaction);
                _context.SaveChangesAsync(cancellationToken);
                
                break;
            case "Reservation":
                var reservation = _context.Reservations.FirstOrDefault(x => x.Id == Guid.Parse(entityId));
                if (reservation == null)
                {
                    throw new KeyNotFoundException($"Reservation with reservation id {entityId} not found");
                }
                
                reservation.IsFullPaid = true;
                _context.Reservations.Update(reservation);
                _context.SaveChangesAsync(cancellationToken);
                
                var reservationTransaction = _context.Transactions.FirstOrDefault(x => x.ReservationId == Guid.Parse(entityId));
                reservationTransaction.TransactionStatus = "Success";
                reservationTransaction.TransactionDate = LocalDateTime.FromDateTime(DateTime.Now);
                _context.Transactions.Update(reservationTransaction);
                _context.SaveChangesAsync(cancellationToken);
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