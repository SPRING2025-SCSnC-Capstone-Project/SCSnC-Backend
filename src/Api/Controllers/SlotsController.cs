using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Slots.Commands;
using Application.Slots.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SlotsController: ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<SlotDto>>> AddSlot([FromBody] AddSlotRequest request) {
        var command = new AddSlotCommand() {
            SlotNumber = request.SlotNumber,
            TimeStart = request.TimeStart,
            TimeEnd = request.TimeEnd
        };

        var result = await Mediator.Send(command);

        return Ok(Result<SlotDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<SlotDto>>> RemoveSlot([FromRoute] Guid id) {
        var command = new RemoveSlotCommand() {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<SlotDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<SlotDto>>> UpdateSlot([FromRoute] Guid id, [FromBody] UpdateSlotRequest request) {
        var command = new UpdateSlotCommand() {
            Id = id,
            SlotNumber = request.SlotNumber,
            TimeStart = request.TimeStart,
            TimeEnd = request.TimeEnd
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<SlotDto>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<SlotDto>>> GetSlotById([FromRoute] Guid id) {
        var query = new GetSlotByIdQuery() {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<SlotDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<SlotDto>>>> GetSlotsPaginated([FromQuery] PaginatedQueryParameters request) {
        var query = new GetSlotsPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<SlotDto>>.Succeed(result));
    }
}