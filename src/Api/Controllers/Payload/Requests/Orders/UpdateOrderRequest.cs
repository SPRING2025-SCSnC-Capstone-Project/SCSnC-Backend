using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Requests.Orders;

public class UpdateOrderRequest
{
    public Guid OrderId { get; set; }
    public List<CreateOrderDetailDto> OrderDetails { get; set; }
}