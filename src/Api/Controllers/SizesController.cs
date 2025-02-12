using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Sizes;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Sizes.Commands.AddSize;
using Application.Sizes.Commands.DeleteSize;
using Application.Sizes.Commands.UpdateSize;
using Application.Sizes.Queries.GetSizeById;
using Application.Sizes.Queries.GetSizesPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SizesController : ApiControllerBase
{
    #region Basic CRUD Operations
    
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<SizeDto>>>> GetSizes([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetSizesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<SizeDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<SizeDto>>> GetSize([FromRoute] Guid id)
    {
        var query = new GetSizeByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<SizeDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<SizeDto>>> AddSize([FromBody] AddSizeRequest request)
    {
        var command = new AddSizeCommand()
        {
            SizeName = request.SizeName,
            PriceAdjustment = request.PriceAdjust
        };

        var result = await Mediator.Send(command);

        return Ok(Result<SizeDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<SizeDto>>> UpdateSize([FromRoute] Guid id, [FromBody] UpdateSizeRequest request)
    {
        var command = new UpdateSizeCommand()
        {
            Id = id,
            SizeName = request.SizeName,
            PriceAdjustment = request.PriceAdjustment
        };

        var result = await Mediator.Send(command);

        return Ok(Result<SizeDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<SizeDto>>> RemoveSize([FromRoute] Guid id)
    {
        var command = new DeleteSizeCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<SizeDto>.Succeed(result));
    }
    
    #endregion
}