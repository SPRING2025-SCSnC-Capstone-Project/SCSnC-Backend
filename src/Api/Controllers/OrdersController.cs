using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Orders;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.UpdateOrder;
using Application.Orders.Queries.GetOrderById;
using Application.Orders.Queries.GetOrdersByBranchPaginated;
using Application.Orders.Queries.GetOrdersByUserPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class OrdersController : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<ResponseOrderDto>>> GetOrderById([FromRoute] Guid id)
    {
        var query = new GetOrderByIdQuery()
        {
            OrderId = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<ResponseOrderDto>.Succeed(result));
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<Result<PaginatedList<ResponseOrderDto>>>> GetOrderByUserId([FromRoute] Guid userId, [FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetOrdersByUserPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            UserId = userId
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<ResponseOrderDto>>.Succeed(result));
    }

    [HttpGet("branch/{branchId:guid}")]
    public async Task<ActionResult<Result<PaginatedList<ResponseOrderDto>>>> GetOrderByBranchId([FromRoute] Guid branchId)
    {
        var query = new GetOrdersByBranchPaginatedQuery()
        {
            BranchId = branchId
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<ResponseOrderDto>>.Succeed(result));
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
            BranchId = request.BranchId,
            ReservationId = request.ReservationId
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