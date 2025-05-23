using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Toppings;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.ToppingPricesAtBranches.Commands.UpdateToppingPriceAtBranch;
using Application.ToppingPricesAtBranches.Queries.GetToppingPriceOfAllBranches;
using Application.Toppings.Commands.AddTopping;
using Application.Toppings.Commands.DeleteTopping;
using Application.Toppings.Commands.UpdateTopping;
using Application.Toppings.Queries.GetToppingById;
using Application.Toppings.Queries.GetToppingsPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ToppingsController: ApiControllerBase
{
    #region Basic CRUD Operations
    
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<ToppingDto>>>> GetToppings([FromQuery] PaginatedToppingQueryParameters request)
    {
        var query = new GetToppingsPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<ToppingDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<ToppingDto>>> GetToppingById([FromRoute] Guid id, [FromBody] ToppingBranchRequest request)
    {
        var query = new GetToppingByIdQuery()
        {
            Id = id,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(query);

        return Ok(Result<ToppingDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<ToppingDto>>> AddTopping([FromBody] AddToppingRequest request)
    {
        var command = new AddToppingCommand()
        {
            ToppingName = request.ToppingName,
            ToppingDescription = request.ToppingDescription,
            Price = request.Price
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ToppingDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<ToppingDto>>> UpdateTopping([FromRoute] Guid id, [FromBody] UpdateToppingRequest request)
    {
        var command = new UpdateToppingCommand()
        {
            Id = id,
            Name = request.ToppingName,
            Description = request.ToppingDescription,
            IsActive = request.IsActive
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ToppingDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<ToppingDto>>> RemoveTopping([FromRoute] Guid id)
    {
        var command = new DeleteToppingCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ToppingDto>.Succeed(result));
    }
    
    #endregion
    
    #region Extra CRUD Operations
    
    [HttpPut("{toppingid:guid}/price")]
    public async Task<ActionResult<Result<ToppingPriceAtBranchDto>>> UpdateToppingPrice([FromRoute] Guid toppingid, [FromBody] UpdateToppingPriceRequest request)
    {
        var command = new UpdateToppingPriceAtBranchCommand()
        {
            ToppingId = toppingid,
            BranchId = request.BranchId,
            Price= request.Price
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ToppingPriceAtBranchDto>.Succeed(result));
    }
    
    [HttpGet("{toppingid:guid}/branch-price")]
    public async Task<ActionResult<Result<List<ToppingPriceAtBranchDto>>>> GetToppingPrice([FromRoute] Guid toppingid)
    {
        var query = new GetToppingPriceOfAllBranchesQuery()
        {
            ToppingId = toppingid
        };

        var result = await Mediator.Send(query);

        return Ok(Result<List<ToppingPriceAtBranchDto>>.Succeed(result));
    }
    
    #endregion
}