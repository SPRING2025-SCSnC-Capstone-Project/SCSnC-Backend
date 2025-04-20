using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Workspaces.Commands;
using Application.Workspaces.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WorkspacesController: ApiControllerBase {
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
            MediaUrls = mediaUrls,
            WorkspaceName = request.WorkspaceName,
            Description = request.Description,
            BranchId = request.BranchId,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceDto>>> RemoveWorkspace([FromRoute] Guid id) {
        var command = new RemoveWorkspaceCommand() {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceDto>>> UpdateWorkspace([FromRoute] Guid id, [FromBody] UpdateWorkspaceRequest request) {
        var command = new UpdateWorkspaceCommand() {
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
    public async Task<ActionResult<Result<WorkspaceDto>>> GetWorkspaceById([FromRoute] Guid id) {
        var query = new GetWorkspaceByIdQuery() {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<WorkspaceDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<WorkspaceDto>>>> GetWorkspacesPaginated([FromQuery] GetWorkspacesPaginatedRequest request) {
        var query = new GetWorkspacesPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            Filter = request.Filter,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<WorkspaceDto>>.Succeed(result));
    }
}
