using Api.Controllers.Payload.Requests.Orders;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.UpdateOrder;
using Application.Orders.Queries.GetOrderById;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class OrdersController: ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<ResponseOrderDto>>> GetOrderById([FromRoute]Guid id)
    {
        var query = new GetOrderByIdQuery()
        {
            OrderId = id
        };
    
        var result = await Mediator.Send(query);
    
        return Ok(Result<ResponseOrderDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<OrderDto>>> AddOrder([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand()
        {
            UserId = request.UserId,
            TableId = request.TableId,
            WorkspaceId = request.WorkspaceId,
            VoucherId = request.VoucherId,
            OrderDetails = request.OrderDetails,
            TotalPrice = request.TotalPrice,
            PaymentMethod = request.PaymentMethod,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<OrderDto>.Succeed(result));
    }
    
    [HttpPut]
    public async Task<ActionResult<Result<OrderDto>>> UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        var command = new UpdateOrderCommand()
        {
            OrderId = request.OrderId,
            OrderDetails = request.OrderDetails
        };
    
        var result = await Mediator.Send(command);
    
        return Ok(Result<OrderDto>.Succeed(result));
    }
}