using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.WorkspaceTypes.Commands;
using Application.WorkspaceTypes.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Api.Controllers;

public class WorkspaceTypesController : ApiControllerBase
{
    [HttpPost]
    [RequestSizeLimit(524288000)]
    public async Task<ActionResult<Result<WorkspaceTypeDto>>> AddWorkspaceType([FromForm] AddWorkspaceTypeRequest request)
    {
        try
        {
            var command = new AddWorkspaceTypeCommand()
            {
                WorkspaceTypeName = request.WorkspaceTypeName,
                WorkspaceTypeDescription = request.WorkspaceTypeDescription,
                MaxCapacity = request.MaxCapacity,
                PricePerHour = request.PricePerHour,
                BranchId = request.BranchId,
                Images = request.Images,
                WorkspaceInWorkspaceTypes = JsonConvert.DeserializeObject<WorkspaceInWorkspaceType[]>(request.WorkspaceInWorkspaceTypes)
            };

            var result = await Mediator.Send(command);

            return Ok(Result<WorkspaceTypeDto>.Succeed(result));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw ex;
        }

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
        try
        {
            Debug.WriteLine(JsonConvert.DeserializeObject<WorkspaceUtilityServiceDto[]>(request.WorkspaceUtilityServices).Length);
            var command = new UpdateWorkspaceTypeCommand()
            {
                Id = id,
                MaxCapacity = request.MaxCapacity ?? null,
                WorkspaceTypeName = request.WorkspaceTypeName ?? null,
                WorkspaceTypeDescription = request.WorkspaceTypeDescription ?? null,
                PricePerHour = request.PricePerHour ?? null,
                WorkspaceUtilityServices = request.WorkspaceUtilityServices != null ? JsonConvert.DeserializeObject<WorkspaceUtilityServiceDto[]>(request.WorkspaceUtilityServices) : [],
                ModelFile = request.ModelFile,
                IsActive = request.IsActive,
                WorkspacesAtBranches = request.WorkspacesAtBranches != null ? JsonConvert.DeserializeObject<WorkspaceTypeAtBranchDto[]>(request.WorkspacesAtBranches) : [],
                WorkspaceInWorkspaceTypes = request.WorkspaceInWorkspaceTypes != null ? JsonConvert.DeserializeObject<WorkspaceInWorkspaceType[]>(request.WorkspaceInWorkspaceTypes) : [],
                UpdateWorkspaceTypeImages = request.UpdateWorkspaceTypeImages != null ? JsonConvert.DeserializeObject<UpdateWorkspaceTypeImage[]>(request.UpdateWorkspaceTypeImages) : [],
                NewImages = request.NewImages ?? [],

            };

            var result = await Mediator.Send(command);

            return Ok(Result<WorkspaceTypeDto>.Succeed(result));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return StatusCode(500, new { error = ex.Message});
        }
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