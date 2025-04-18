using Api.Controllers.Payload.Requests;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Events.Commands;
using Application.Events.Queries.GetEventById;
using Application.Events.Queries.GetEventsByUserPaginated;
using Application.Events.Queries.GetEventsPaginated;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventsController : ApiControllerBase {

    private readonly IApplicationDbContext _context;

    public EventsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Result<EventDto>>> CreateEvent([FromBody] CreateEventRequest request) {
        var command = new AddEventCommand() {
            EventTitle = request.EventTitle,
            EntranceFee = request.EntranceFee,
            EventDescription = request.EventDescription,
            ReservationId = request.ReservationId,
            UserId = request.CurrentUserId,
            SlotIds = request.SlotIds,
            IsEventPrivate = request.IsEventPrivate
        };

        var result = await Mediator.Send(command);
        return Ok(Result<EventDto>.Succeed(result));
    }

    [HttpGet("{eventid:guid}")]
    public async Task<ActionResult<Result<EventDto>>> GetEventById([FromRoute] Guid eventid) {
        var query = new GetEventByIdQuery() {
            Id = eventid
        };

        var result = await Mediator.Send(query);
        return Ok(Result<EventDto>.Succeed(result));
    }

    [HttpGet("user/{userid:guid}")]
    public async Task<ActionResult<Result<PaginatedList<EventDto>>>> GetEventsByUserPaginated([FromRoute] Guid userid, [FromQuery] GetEventsPaginatedRequest request) {
        var command = new GetEventsByUserPaginatedQuery() {
            UserId = userid,
            Page = request.Page,
            Size = request.Size,
            Filter = request.Filter,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<EventDto>>.Succeed(result));
    }

    [HttpGet()]
    public async Task<ActionResult<Result<PaginatedList<EventDto>>>> GetEventsPaginated([FromQuery] GetEventsPaginatedRequest request) {
        var command = new GetEventsPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            Filter = request.Filter,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<EventDto>>.Succeed(result));
    }
}
