using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.WorkspaceTypes.Commands;
using Application.WorkspaceTypes.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers;

public class WorkspaceTypesController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> AddWorkspaceType([FromBody] AddWorkspaceTypeRequest request)
    {
        var mediaTypes = new List<string>();
        var mediaUrls = new List<string>();

        for (var i = 0; i < request.WorkspaceMedias.Length; i++)
        {
            mediaTypes.Add(request.WorkspaceMedias[i].MediaType);
            mediaUrls.Add(request.WorkspaceMedias[i].MediaUrl);
        }

        var command = new AddWorkspaceTypeCommand()
        {
            WorkspaceTypeName = request.WorkspaceTypeName,
            MaxCapacity = request.MaxCapacity,
            PricePerHour = request.PricePerHour,
            MediaTypes = mediaTypes,
            MediaUrls = mediaUrls
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> RemoveWorkspaceType([FromRoute] Guid id)
    {
        var command = new RemoveWorkspaceTypeCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    [RequestSizeLimit(524288000)]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> UpdateWorkspaceType([FromRoute] Guid id, [FromForm] UpdateWorkspaceTypeRequest request)
    {
        Debug.WriteLine(request.WorkspaceUtilityServices);
        var command = new UpdateWorkspaceTypeCommand()
        {
            Id = id,
            MaxCapacity = request.MaxCapacity ?? null,
            WorkspaceTypeName = request.WorkspaceTypeName ?? null,
            PricePerHour = request.PricePerHour ?? null,
            WorkspaceUtilityServices = request.WorkspaceUtilityServices,
            ModelFile = request.ModelFile ?? null,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> GetWorkspaceTypeById([FromRoute] Guid id)
    {
        var query = new GetWorkspaceTypeByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<WorkspaceTypeDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<WorkspaceTypeDto>>>> GetWorkspaceTypesPaginated([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetWorkspaceTypesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);
        return Ok(Result<PaginatedList<WorkspaceTypeDto>>.Succeed(result));
    }
}