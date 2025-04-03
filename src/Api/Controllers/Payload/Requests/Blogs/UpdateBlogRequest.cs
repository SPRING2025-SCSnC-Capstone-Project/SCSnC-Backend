using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Requests.Blogs;

public class UpdateBlogRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Guid> RemoveMedia { get; set; }
    public List<AddBlogMediaDto> AddMedia { get; set; }
}