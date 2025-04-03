using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Blogs.Commands.CreateBlog;

public record CreateBlogCommand : IRequest<BlogDto>
{
    public Guid EventId { get; init; }
    public Guid UserId { get; init; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<AddBlogMediaDto> Media { get; set; }
}

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, BlogDto> 
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CreateBlogCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = new Blog
        {
            EventId = request.EventId,
            UserId = request.UserId,
            Title = request.Title,
            Content = request.Content,
            IsActive = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
        };
        
        await _context.Blogs.AddAsync(blog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        foreach (var mediaInput in request.Media)
        {
            var blogMedia = new BlogMedia
            {
                BlogId = blog.Id,
                MediaType = mediaInput.MediaType,
                MediaUrl = mediaInput.MediaUrl,
                IsActive = true,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
            };
            
            await _context.BlogMedias.AddAsync(blogMedia, cancellationToken);
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.Blogs
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.BlogMedias)
            .FirstOrDefaultAsync(x => x.Id == blog.Id, cancellationToken);
            
        
        return _mapper.Map<BlogDto>(result);
    }
}