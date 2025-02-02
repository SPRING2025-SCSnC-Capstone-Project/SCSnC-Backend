using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Tables;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Tables.Commands;
using Application.Tables.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TablesController: ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<TableDto>>> AddTable([FromBody] AddTableRequest request) {
        var command = new AddTableCommand() {
            TableNumber = request.TableNumber,
            SeatAmount = request.SeatAmount
        };

        var result = await Mediator.Send(command);

        return Ok(Result<TableDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<TableDto>>> RemoveTable([FromRoute] Guid id) {
        var command = new RemoveTableCommand() {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<TableDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<TableDto>>> UpdateTable([FromRoute] Guid id, [FromBody] UpdateTableRequest request) {
        var command = new UpdateTableCommand() {
            Id = id,
            TableNumber = request.TableNumber,
            SeatAmount = request.SeatAmount,
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<TableDto>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<TableDto>>> GetTableById([FromRoute] Guid id) {
        var query = new GetTableByIdQuery() {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<TableDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<TableDto>>>> GetTablesPaginated([FromQuery] PaginatedQueryParameters request) {
        var query = new GetTablesPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<TableDto>>.Succeed(result));
    }
}