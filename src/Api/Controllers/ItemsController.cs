using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Items;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Items.Commands.AddItem;
using Application.Items.Commands.DeleteItem;
using Application.Items.Commands.UpdateItem;
using Application.Items.Queries.GetItemById;
using Application.Items.Queries.GetItemsPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ItemsController : ApiControllerBase
{
    #region Basic CRUD Operations
    
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<ItemDto>>>> GetItems([FromQuery] PaginatedItemsQueryParams request)
    {
        var query = new GetItemsPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            FilterByCategory = request.FilterByCategory
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<ItemDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<ItemDto>>> GetItemById([FromRoute] Guid id)
    {
        var query = new GetItemByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<ItemDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<ItemDto>>> AddItem([FromBody] AddItemRequest request)
    {
        var command = new AddItemCommand()
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Img = request.Img,
            SizeIds = request.SizeIds
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<ItemDto>>> UpdateItem([FromRoute] Guid id, [FromBody] UpdateItemRequest request)
    {
        var command = new UpdateItemCommand()
        {
            Id = id,
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Img = request.Img,
            IsActive = request.IsActive
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<ItemDto>>> RemoveItem([FromRoute] Guid id)
    {
        var command = new DeleteItemCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemDto>.Succeed(result));
    }
    
    #endregion
}