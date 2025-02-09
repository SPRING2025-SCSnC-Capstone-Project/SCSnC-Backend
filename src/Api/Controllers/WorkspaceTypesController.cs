using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.WorkspaceTypes.Commands;
using Application.WorkspaceTypes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WorkspaceTypesController: ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> AddWorkspaceType([FromBody] AddWorkspaceTypeRequest request) {
        var command = new AddWorkspaceTypeCommand() {
            WorkspaceTypeName = request.WorkspaceTypeName,
            MaxCapacity = request.MaxCapacity
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> RemoveWorkspaceType([FromRoute] Guid id) {
        var command = new RemoveWorkspaceTypeCommand() {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> UpdateWorkspaceType([FromRoute] Guid id, [FromBody] UpdateWorkspaceTypeRequest request) {
        var command = new UpdateWorkspaceTypeCommand() {
            Id = id,
            MaxCapacity = request.MaxCapacity,
            WorkspaceTypeName = request.WorkspaceTypeName,
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> GetWorkspaceTypeById([FromRoute] Guid id) {
        var query = new GetWorkspaceTypeByIdQuery() {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<WorkspaceTypeDto>>>> GetWorkspaceTypesPaginated([FromQuery] PaginatedQueryParameters request) {
        var query = new GetWorkspaceTypesPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<WorkspaceTypeDto>>.Succeed(result));
    }
}