using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Reservations.Commands;
using Application.Reservations.Queries.GetReservationById;
using Application.Reservations.Queries.GetReservationsByUserPaginated;
using Application.Reservations.Queries.GetReservationsPaginated;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers;

public class ReservationsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<ResponseReservationDto>>> CreateReservation([FromBody] CreateReservationRequest request)
    {
        Debug.WriteLine(request.ReservationDate);
        var command = !request.includeEvent ? new CreateReservationCommand()
        {
            ReservationDate = request.ReservationDate,
            TotalPrice = request.TotalPrice,
            Deposit = request.Deposit,
            WorkspaceTypeId = request.WorkspaceTypeId,
            WorkspaceId = request.WorkspaceId,
            UserId = request.UserId,
            Note = request.Note,
            Email = request.Email,
            Phone = request.Phone,
            SlotIds = request.SlotIds,
            PaymentMethod = request.PaymentMethod,
            IsEventPrivate = request.IsEventPrivate,
            BranchId = request.BranchId,
            CoverImageLink = request.CoverImageLink,
            BookingWithTime = request.BookingWithTime,
            TimeStart = request.TimeStart,
            TimeEnd = request.TimeEnd
        } :
        new CreateReservationCommand()
        {
            ReservationDate = request.ReservationDate,
            TotalPrice = request.TotalPrice,
            Deposit = request.Deposit,
            WorkspaceTypeId = request.WorkspaceTypeId,
            WorkspaceId = request.WorkspaceId,
            UserId = request.UserId,
            Note = request.Note,
            Email = request.Email,
            Phone = request.Phone,
            includeEvent = request.includeEvent,
            EntranceFee = request.EntranceFee,
            EventDescription = request.EventDescription,
            EventTitle = request.EventTitle,
            SlotIds = request.SlotIds,
            PaymentMethod = request.PaymentMethod,
            IsEventPrivate = request.IsEventPrivate,
            BranchId = request.BranchId,
            CoverImageLink = request.CoverImageLink,
            BookingWithTime = request.BookingWithTime,
            TimeStart = request.TimeStart,
            TimeEnd = request.TimeEnd
        };

        var result = await Mediator.Send(command);
        return Ok(Result<ResponseReservationDto>.Succeed(result));
    }

    [HttpGet("{reservationid:guid}")]
    public async Task<ActionResult<Result<ReservationDto>>> GetReservationById([FromRoute] Guid reservationid)
    {
        var query = new GetReservationByIdQuery()
        {
            Id = reservationid
        };

        var result = await Mediator.Send(query);
        return Ok(Result<ReservationDto>.Succeed(result));
    }

    [HttpGet("user/{userid:guid}")]
    public async Task<ActionResult<Result<PaginatedList<ReservationDto>>>> GetReservationsByUserPaginated([FromRoute] Guid userid, [FromQuery] GetReservationsPaginatedRequest request)
    {
        var command = new GetReservationsByUserPaginatedQuery()
        {
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
    public async Task<ActionResult<Result<PaginatedList<ReservationDto>>>> GetReservationsPaginated([FromQuery] GetReservationsPaginatedRequest request)
    {
        var command = new GetReservationsPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            Filter = request.Filter,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            GetAllReservationByBranch = request.GetAllReservationByBranch,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<ReservationDto>>.Succeed(result));
    }

    [HttpPut("cancel/{id:guid}")]
    public async Task<ActionResult<Result<ResponseReservationDto>>> CancelReservation([FromRoute] Guid id)
    {
        var command = new CancelReservationCommand()
        {
            ReservationId = id,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<ResponseReservationDto>.Succeed(result));
    }

}
