using Api.Controllers.Payload.Requests;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Events.Commands;
using AutoMapper;
using Domain.Entities;
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
            UserId = request.UserId,
            EventTitle = request.EventTitle,
            EntranceFee = request.EntranceFee,
            NumberOfPeople = request.NumberOfPeople,
            EventFee = request.EventFee,
            WorkspaceId = request.WorkspaceId,
            EventEndDate = request.EventEndDate,
            EventStartDate = request.EventStartDate,
            EventDescription = request.EventDescription
        };

        var result = await Mediator.Send(command);
        return Ok(Result<EventDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<IEnumerable<Event>> GetEvents()
    {
        return _context.Events.ToList().Count > 0 ? _context.Events.AsEnumerable() : Enumerable.Empty<Event>();
    }
}
