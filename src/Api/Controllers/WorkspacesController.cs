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
        var mediaTypes = new List<string>();
        var mediaUrls = new List<string>();

        for (var i = 0; i < request.WorkspaceMedias.Length; i++) {
            mediaTypes.Add(request.WorkspaceMedias[i].MediaType);
            mediaUrls.Add(request.WorkspaceMedias[i].MediaUrl);
        }
        
        var command = new AddWorkspaceCommand() {
            WorkspaceNumber = request.WorkspaceNumber,
            WorkspaceTypeId = request.WorkspaceTypeId,
            MediaTypes = mediaTypes,
            MediaUrls = mediaUrls
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
            SlotIds = request.SlotIds
        };

        var result = await Mediator.Send(query);
        return Ok(Result<List<WorkspaceDto>>.Succeed(result));
    }
}