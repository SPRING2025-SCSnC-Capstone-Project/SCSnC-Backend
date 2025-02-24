using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Requests.Orders;

public class CreateOrderRequest
{
    public Guid? TableId { get; set; }
    public Guid? WorkspaceId { get; set; }
    public Guid UserId { get; set; }
    public Guid? VoucherId { get; set; }
    public List<CreateOrderDetailDto> OrderDetails { get; set; }
    public double TotalPrice { get; set; }
    public string PaymentMethod { get; set; }
}