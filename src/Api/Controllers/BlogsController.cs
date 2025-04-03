using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Blogs;
using Application.Blogs.Commands.CreateBlog;
using Application.Blogs.Commands.DeleteBlog;
using Application.Blogs.Commands.UpdateBlog;
using Application.Blogs.Queries.GetBlogById;
using Application.Blogs.Queries.GetBlogsPaginated;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BlogsController: ApiControllerBase
{
    #region Basic CRUD Operations
    
    [HttpGet]
    public async Task<ActionResult<PaginatedList<BlogDto>>> GetBlogs([FromQuery] PaginatedQueryParameters queryParameters)
    {
        var query = new GetBlogsPaginatedQuery()
        {
            Page = queryParameters.Page,
            Size = queryParameters.Size,
            SortBy = queryParameters.SortBy,
            SortOrder = queryParameters.SortOrder
        };

        var result = await Mediator.Send(query);
        
        return Ok(Result<PaginatedList<BlogDto>>.Succeed(result));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BlogDto>> GetBlogById([FromRoute]Guid id)
    {
        var query = new GetBlogByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);
        
        return Ok(Result<BlogDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<BlogDto>> CreateBlog([FromBody] CreateBlogRequest request)
    {
        var command = new CreateBlogCommand()
        {
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            EventId = request.EventId,
            Media = request.Media
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<BlogDto>.Succeed(result));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BlogDto>> UpdateBlog([FromRoute]Guid id, [FromBody] UpdateBlogRequest request)
    {
        var command = new UpdateBlogCommand()
        {
            BlogId = id,
            Title = request.Title,
            Content = request.Content,
            RemoveMedia = request.RemoveMedia,
            Media = request.AddMedia
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<BlogDto>.Succeed(result));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BlogDto>> DeleteBlog([FromRoute]Guid id)
    {
        var command = new DeleteBlogCommand()
        {
            BlogId = id
        };

        var result = await Mediator.Send(command);
        
        return Ok(Result<BlogDto>.Succeed(result));
    }
    
    #endregion
}