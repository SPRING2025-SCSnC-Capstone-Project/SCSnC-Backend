using Api.Controllers.Payload.Requests;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Workspaces.Commands;
using Application.Workspaces.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Api.Controllers;

public class WorkspacesController : ApiControllerBase
{
    private IApplicationDbContext _context;

    public WorkspacesController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Result<WorkspaceDto>>> AddWorkspace([FromBody] AddWorkspaceRequest request) { 
        var command = new AddWorkspaceCommand() {
            WorkspaceNumber = request.WorkspaceNumber,
            WorkspaceTypeAtBranchId = request.WorkspaceTypeAtBranchId,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceDto>>> RemoveWorkspace([FromRoute] Guid id)
    {
        var command = new RemoveWorkspaceCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceDto>>> UpdateWorkspace([FromRoute] Guid id, [FromBody] UpdateWorkspaceRequest request)
    {
        var command = new UpdateWorkspaceCommand()
        {
            Id = id,
            WorkspaceNumber = request.WorkspaceNumber,
            WorkspaceTypeId = request.WorkspaceTypeId,
            WorkspaceName = request.WorkspaceName,
            Description = request.Description,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceDto>>> GetWorkspaceById([FromRoute] Guid id)
    {
        var query = new GetWorkspaceByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<WorkspaceDto>>>> GetWorkspacesPaginated([FromQuery] GetWorkspacesPaginatedRequest request)
    {
        var query = new GetWorkspacesPaginatedQuery()
        {
            Page = request.Page,
            Size = 1000,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            Filter = request.Filter,
            SlotNumber = request.SlotNumber,
            ReserveDate = request.ReserveDate,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<WorkspaceDto>>.Succeed(result));
    }

    [HttpPost("find-available-workspaces")]
    public async Task<ActionResult<Result<List<WorkspaceDto>>>> GetWorkspacesByTimePaginated([FromBody] GetWorkspacesByTimeAndTypePaginatedRequest request)
    {
        var query = new GetWorkspacesByTimeAndTypeQuery()
        {
            WorkspaceTypeId = request.WorkspaceTypes,
            ReservationDate = request.ReserveDate,
            SlotIds = request.SlotIds,
            BranchId = request.BranchId,
            BookingWithTime = request.BookingWithTime,
            TimeEnd = request.TimeEnd,
            TimeStart = request.TimeStart,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<List<WorkspaceDto>>.Succeed(result));
    }
}