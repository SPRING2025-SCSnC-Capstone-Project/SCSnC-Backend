using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Application.Orders.Commands.CreateOrder;

#region Code flow: create order and return link, calculate total price, including order details and discount and store data on BE, FE doesn't need to calculate
//
// public record CreateOrderCommand : IRequest<OrderDto>
// {
//     public Guid? TableId { get; init; }
//     public Guid? WorkspaceId { get; init; }
//     public Guid UserId { get; init; }
//     public Guid? VoucherId { get; init; }
//     public List<CreateOrderDetailDto> OrderDetails { get; init; }
// }
//
// public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly IMapper _mapper;
//     private readonly IPaymentService _vnpayService;
//
//     public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper, IPaymentService vnPayService)
//     {
//         _context = context;
//         _mapper = mapper;
//         _vnpayService = vnPayService;
//     }
//
//     
//     public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
//     {
//         #region Manual Validation (temporary)
//         //double totalPrice = 0;
//         // var checkOrder = _context.Orders.OrderByDescending(x => x.LastUpdatedAt).FirstOrDefaultAsync(x => x.TableId == request.TableId, cancellationToken).Result;
//         //
//         // if (checkOrder != null && checkOrder.PaymentStatus == false)
//         // {
//         //     throw new ValidationException("Table is already occupied");
//         //     //redirect to update order
//         //     
//         // }
//         
//         // Upper comment is code for checking if table is already occupied
//         
//         // Will need another way to validate order either have tableId or workspaceId
//         // if both are null, throw exception
//         if ((request.TableId.HasValue == false) && (request.WorkspaceId.HasValue == false))
//         {
//             throw new ValidationException("TableId or WorkspaceId must be provided");
//         }
//         // if both are not null, throw exception
//         if ((request.TableId.HasValue == false) && (request.WorkspaceId.HasValue == false))
//         {
//             throw new ValidationException("TableId or WorkspaceId must be provided");
//         }
//         
//         #endregion
//         
//         // start creating order from here
//         
//         var order = new Order
//         {
//             TableId = request.TableId,
//             WorkspaceId = request.WorkspaceId,
//             UserId = request.UserId,
//             VoucherId = request.VoucherId,
//             TotalPrice = 0,
//             
//             IsActive = true,
//             CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
//             LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
//             PaymentStatus = false
//         };
//
//         await _context.Orders.AddAsync(order, cancellationToken);
//         await _context.SaveChangesAsync(cancellationToken);
//
//         foreach (var orderDetail in request.OrderDetails)
//         {
//             double orderDetailPrice = 0;
//             
//             var newOrderDetail = new OrderDetail
//             {
//                 ItemWithSizeId = _context.ItemWithSizes.FirstOrDefaultAsync(x =>
//                     x.ItemId == orderDetail.ItemId && x.SizeId == orderDetail.SizeId, cancellationToken).Result.Id,
//                 OrderId = order.Id,
//                 Quantity = orderDetail.Quantity,
//                 TotalPrice = orderDetailPrice
//             };
//             
//             await _context.OrderDetails.AddAsync(newOrderDetail, cancellationToken);
//             await _context.SaveChangesAsync(cancellationToken);
//             
//             var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == orderDetail.ItemId, cancellationToken);
//             var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == orderDetail.SizeId, cancellationToken);
//             if (item is null)
//             {
//                 throw new KeyNotFoundException($"Item with id {orderDetail.ItemId} not found");
//             }
//             
//             orderDetailPrice += item.ItemBasePrice + size.PriceAdjustment;
//
//             foreach (var includeTopping in orderDetail.ToppingIds)
//             {
//                 var topping =
//                     await _context.Toppings.FirstOrDefaultAsync(x => x.Id == includeTopping, cancellationToken);
//                 if (topping is null)
//                 {
//                     throw new KeyNotFoundException($"IncludeTopping with id {topping} not found");
//                 }
//                 orderDetailPrice += topping.Price;
//
//                 var newincludeTopping = new IncludeTopping
//                 {
//                     OrderDetailId = newOrderDetail.Id,
//                     ToppingId = topping.Id
//                 };
//                 
//                 await _context.IncludeToppings.AddAsync(newincludeTopping, cancellationToken);
//                 await _context.SaveChangesAsync(cancellationToken);
//             }
//
//             newOrderDetail.TotalPrice = orderDetailPrice * orderDetail.Quantity;
//             _context.OrderDetails.Update(newOrderDetail);
//             await _context.SaveChangesAsync(cancellationToken);
//         }
//         var discount = await _context.UserVouchers.Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Id == request.VoucherId, cancellationToken);
//         
//         if (discount != null)
//         {
//             if (discount.RedeemStatus == true || discount.Voucher.ExpiredDate < LocalDateTime.FromDateTime(DateTime.Now))
//             {
//                 order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result;
//             }
//             order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result / 100 * (100 - discount.Voucher.DiscountValue);
//         } 
//         else order.TotalPrice = OrderHelper.CalculateTotalPrice(order.Id, cancellationToken, _context).Result;
//         
//         _context.Orders.Update(order);
//         
//         await _context.SaveChangesAsync(cancellationToken);
//         
//         var get = await _context.Orders
//             .Include(o => o.Table)
//             .Include(o => o.User)
//             .Include(o => o.Voucher)
//             .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);
//
//         var result = _mapper.Map<OrderDto>(get);
//         
//         result.OrderDetails = _context.OrderDetails
//             .Include(od => od.ItemWithSize)
//             .Include(od => od.ItemWithSize.Item)
//             .Include(od => od.ItemWithSize.Size)
//             .Include(od => od.IncludeToppings)
//             .ThenInclude(t => t.Topping)
//             .Where(od => od.OrderId == order.Id)
//             .Select(od => _mapper.Map<OrderDetailDto>(od))
//             .ToList();
//         
//         //create payment here
//         
//         VNPayConfig vnPayConfig = VNPayHelper.GetConfigData();
//
//         VNPayRequest vnPayRequest = new VNPayRequest()
//         {
//             vnp_Version = vnPayConfig.Version,
//             vnp_TmnCode = vnPayConfig.TmnCode,
//             vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
//             vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
//             vnp_Amount = /*(int)Math.Ceiling(orderInfo.TotalPrice) * 100*/ (decimal) get.TotalPrice * 100,
//             vnp_CurrCode = vnPayConfig.CurrencyCode,
//             vnp_OrderType = "other",
//             vnp_OrderInfo = $"Date: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Total Price: {get.TotalPrice}",
//             vnp_ReturnUrl = vnPayConfig.ReturnUrl,
//             vnp_TxnRef = order.Id.ToString(),
//             vnp_Command = "pay",
//             vnp_Locale = vnPayConfig.Locale
//         };
//         
//         var paymentUrl = await _vnpayService.GetPaymentLink(vnPayConfig.PaymentUrl, vnPayConfig.HashSecret, vnPayRequest);
//         
//         result.PaymentLink = paymentUrl;
//         
//         return result;
//     }
// }

#endregion


#region Code flow: create order and return payment link on BE, calculate total price, including order details and discount on FE and send to BE for storing data

public record CreateOrderCommand : IRequest<OrderDto>
{
    public Guid? TableId { get; init; }
    public Guid? WorkspaceId { get; init; }
    public Guid UserId { get; init; }
    public Guid? VoucherId { get; init; }
    public double TotalPrice { get; init; }
    public List<CreateOrderDetailDto> OrderDetails { get; init; }
    public string PaymentMethod { get; init;  }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
     private readonly IMapper _mapper;
     private readonly IPaymentService _vnpayService;

     public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper, IPaymentService vnPayService)
     {
         _context = context;
         _mapper = mapper;
         _vnpayService = vnPayService;
     }
     
     public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
     {
         #region Manual Validation (temporary)
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
         
         //validate voucher used or expired
         var uservoucher = await _context.UserVouchers
             .Include(x => x.Voucher)
             .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.VoucherId == request.VoucherId, cancellationToken);
         
         if (uservoucher.RedeemStatus == true || uservoucher.Voucher.ExpiredDate <= LocalDateTime.FromDateTime(DateTime.Now))
         {
             throw new ValidationException("Voucher is used or expired");
         }
         
         #endregion
         
         var order = new Order
         {
             TableId = request.TableId,
             WorkspaceId = request.WorkspaceId,
             UserId = request.UserId,
             VoucherId = request.VoucherId,
             TotalPrice = request.TotalPrice,
             
             IsActive = true,
             CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
             LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
             PaymentStatus = false
         };
         
         await _context.Orders.AddAsync(order, cancellationToken);
         await _context.SaveChangesAsync(cancellationToken);
         
         foreach (var orderDetail in request.OrderDetails)
         {
             var newOrderDetail = new OrderDetail
             {
                 ItemWithSizeId = _context.ItemWithSizes.FirstOrDefaultAsync(x =>
                     x.ItemId == orderDetail.ItemId && x.SizeId == orderDetail.SizeId, cancellationToken).Result.Id,
                 OrderId = order.Id,
                 Quantity = orderDetail.Quantity,
                 TotalPrice = orderDetail.OrderDetailPrice
             };
             
             await _context.OrderDetails.AddAsync(newOrderDetail, cancellationToken);
             await _context.SaveChangesAsync(cancellationToken);
             
             foreach (var includeTopping in orderDetail.ToppingIds)
             {
                 var newincludeTopping = new IncludeTopping
                 {
                     OrderDetailId = newOrderDetail.Id,
                     ToppingId = includeTopping
                 };
                 
                 await _context.IncludeToppings.AddAsync(newincludeTopping, cancellationToken);
                 await _context.SaveChangesAsync(cancellationToken);
             }
         }
         
         var get = await _context.Orders
             .Include(o => o.Table)
             .Include(o => o.User)
             .Include(o => o.Voucher)
             // .Include(o => o.OrderDetails)
             // .ThenInclude(od => od.ItemWithSizes)
             .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

         var result = _mapper.Map<OrderDto>(get);
         
         result.OrderDetails = _context.OrderDetails
             .Include(od => od.ItemWithSize)
             .Include(od => od.ItemWithSize.Item)
             .Include(od => od.ItemWithSize.Size)
             .Include(od => od.IncludeToppings)
             .ThenInclude(t => t.Topping)
             .Where(od => od.OrderId == order.Id)
             .Select(od => _mapper.Map<OrderDetailDto>(od))
             .ToList();
         
         //create payment here

         var payment = new Payment
         {
             Amount = result.TotalPrice,
             PaymentMethod = request.PaymentMethod
         };
         
         await _context.Payments.AddAsync(payment, cancellationToken);
         await _context.SaveChangesAsync(cancellationToken);

         switch (request.PaymentMethod)
         {
             case "VNPay":
                 VNPayConfig vnPayConfig = VNPayHelper.GetConfigData();

                 VNPayRequest vnPayRequest = new VNPayRequest()
                 {
                     vnp_Version = vnPayConfig.Version,
                     vnp_TmnCode = vnPayConfig.TmnCode,
                     vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                     vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
                     vnp_Amount = (decimal) get.TotalPrice * 100,
                     vnp_CurrCode = vnPayConfig.CurrencyCode,
                     vnp_OrderType = "other",
                     vnp_OrderInfo = $"Date: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Total Price: {get.TotalPrice}",
                     vnp_ReturnUrl = vnPayConfig.ReturnUrl,
                     vnp_TxnRef = order.Id.ToString(),
                     vnp_Command = "pay",
                     vnp_Locale = vnPayConfig.Locale
                 };
         
                 var paymentUrl = await _vnpayService.GetPaymentLink(vnPayConfig.PaymentUrl, vnPayConfig.HashSecret, vnPayRequest);
                 result.PaymentLink = paymentUrl;
                 break;
             
             case "Cash":
                 result.PaymentLink = string.Empty;
                 break;
         }
         
         var transaction = new Transaction
         {
             OrderId = order.Id,
             PaymentId = payment.Id,
             TransactionStatus = "Pending",
             TransactionDate = LocalDateTime.FromDateTime(DateTime.Now)
         };
         
         await _context.Transactions.AddAsync(transaction, cancellationToken);
         await _context.SaveChangesAsync(cancellationToken);
         
         return result;
     }
}

#endregion