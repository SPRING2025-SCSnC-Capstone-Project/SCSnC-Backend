using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Branches;
using Application.Branches.Commands.CreateBranch;
using Application.Branches.Commands.DeleteBranch;
using Application.Branches.Commands.UpdateBranch;
using Application.Branches.Queries.GetBranchById;
using Application.Branches.Queries.GetBranchesPaginated;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BranchesController: ApiControllerBase
{
    #region Basic CRUD Operations
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<BranchDto>>>> GetBranches([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetBranchesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<BranchDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> GetBranchById([FromRoute] Guid id)
    {
        var query = new GetBranchByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<BranchDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<BranchDto>>> AddBranch([FromBody] AddBranchRequest request)
    {
        var command = new CreateBranchCommand()
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.Phone,
            Email = request.Email,
            ImgUrl = request.ImgUrl,
            Description = request.Description
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> UpdateBranch([FromRoute] Guid id, [FromBody] UpdateBranchRequest request)
    {
        var command = new UpdateBranchCommand()
        {
            Id = id,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            ImgUrl = request.ImgUrl,
            Description = request.Description
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> DeleteBranch([FromRoute] Guid id)
    {
        var command = new DeleteBranchCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }
    #endregion
}