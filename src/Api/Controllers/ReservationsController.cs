using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Reservations;
using Application.Reservations.Commands;
using Application.Reservations.Queries.GetReservationById;
using Application.Reservations.Queries.GetReservationsByUserPaginated;
using Application.Reservations.Queries.GetReservationsPaginated;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ReservationsController : ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<ResponseReservationDto>>> CreateReservation([FromBody] CreateReservationRequest request) {
        var command = new CreateReservationCommand() {
            ReservationDate = request.ReservationDate,
            TotalPrice = request.TotalPrice,
            Deposit = request.Deposit,
            WorkspaceId = request.WorkspaceId,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserId = request.UserId,
            SlotIds = request.SlotIds,
            WorkspaceUtilityServiceIds = request.WorkspaceUtilityServiceIds,
            PaymentMethod = request.PaymentMethod
        };

        var result = await Mediator.Send(command);
        return Ok(Result<ResponseReservationDto>.Succeed(result));
    }

    [HttpGet("{reservationid:guid}")]
    public async Task<ActionResult<Result<ReservationDto>>> GetReservationById([FromRoute] Guid reservationid) {
        var query = new GetReservationByIdQuery() {
            Id = reservationid
        };

        var result = await Mediator.Send(query);
        return Ok(Result<ReservationDto>.Succeed(result));
    }

    [HttpGet("user/{userid:guid}")]
    public async Task<ActionResult<Result<PaginatedList<ReservationDto>>>> GetReservationsByUserPaginated([FromRoute] Guid userid, [FromQuery] GetReservationsPaginatedRequest request) {
        var command = new GetReservationsByUserPaginatedQuery() {
            UserId = userid,
            Page = request.Page,
            Size = request.Size,
            Filter = request.Filter,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<ReservationDto>>.Succeed(result));
    }

    [HttpGet()]
    public async Task<ActionResult<Result<PaginatedList<ReservationDto>>>> GetReservationsPaginated([FromQuery] GetReservationsPaginatedRequest request) {
        var command = new GetReservationsPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            Filter = request.Filter,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<ReservationDto>>.Succeed(result));
    }
    
    [HttpPut("{reservationid:guid}")]
    public async Task<ActionResult<Result<ReservationDto>>> UpdateReservation([FromRoute] Guid reservationid, [FromBody] UpdateReservationRequest request) {
        var command = new UpdateReservationCommand() {
            Id = reservationid,
            Status = request.Status,
            IsFullPaid = request.IsFullPaid,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<ReservationDto>.Succeed(result));
    }
}
