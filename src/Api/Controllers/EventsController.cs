using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Events.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventsController : ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<EventDto>>> CreateEvent([FromBody] CreateEventRequest request) {
        var command = new AddEventCommand() {
            UserId = request.UserId,
            EventTitle = request.EventTitle,
            EntranceFee = request.EntranceFee,
            WorkspaceId = request.WorkspaceId,
            EventEndDate = request.EventEndDate,
            EventStartDate = request.EventStartDate,
            EventDescription = request.EventDescription
        };

        var result = await Mediator.Send(command);
        return Ok(Result<EventDto>.Succeed(result));
    }
}
