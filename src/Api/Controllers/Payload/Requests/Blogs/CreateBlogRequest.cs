using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Requests.Blogs;

public class CreateBlogRequest
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<AddBlogMediaDto> Media { get; set; }
}