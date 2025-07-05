using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.ItemCategories;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.ItemCategories.Commands.CreateItemCategory;
using Application.ItemCategories.Commands.DeleteItemCategory;
using Application.ItemCategories.Commands.UpdateItemCategory;
using Application.ItemCategories.Queries.GetItemCategoriesPaginated;
using Application.ItemCategories.Queries.GetItemCategoryById;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CategoriesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<ItemCategoryDto>>>> GetItemCategories([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetItemCategoriesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<ItemCategoryDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<SingleItemCategoryDto>>> GetItemCategory([FromRoute] Guid id)
    {
        var query = new GetItemCategoryByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<SingleItemCategoryDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<ItemCategoryDto>>> CreateItemCategory([FromBody] CreateItemCategoryRequest request)
    {
        var command = new CreateItemCategoryCommand()
        {
            Name = request.CategoryName,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemCategoryDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<ItemCategoryDto>>> UpdateItemCategory([FromRoute] Guid id, [FromBody] UpdateItemCategoryRequest request)
    {
        var command = new UpdateItemCategoryCommand()
        {
            Id = id,
            Name = request.CategoryName,
            IsActive = request.IsActive,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemCategoryDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<ItemCategoryDto>>> DeleteItemCategory([FromRoute] Guid id)
    {
        var command = new DeleteItemCategoryCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ItemCategoryDto>.Succeed(result));
    }
}