using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Blogs.Commands.UpdateBlog;

public record UpdateBlogCommand : IRequest<BlogDto>
{
    public Guid BlogId { get; init; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Guid> RemoveMedia { get; set; }
    public List<AddBlogMediaDto> Media { get; set; }
}

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, BlogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UpdateBlogCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BlogDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _context.Blogs
            .FirstOrDefaultAsync(x => x.Id == request.BlogId, cancellationToken);
        var blogMedias = await _context.BlogMedias
            .Where(x => x.BlogId == request.BlogId)
            .ToListAsync(cancellationToken);
        
        //update blog here
        blog.Title = request.Title;
        blog.Content = request.Content;
        blog.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Blogs.Update(blog);
        _context.SaveChangesAsync(cancellationToken);
        //end
        
        //update blog media here
        foreach (var removeMedia in request.RemoveMedia)
        {
            var media = blogMedias.FirstOrDefault(x => x.Id == removeMedia);
            if (media is not null)
            {
                _context.BlogMedias.Remove(media);
            }
        }
        _context.SaveChangesAsync(cancellationToken);
        
        foreach (var media in request.Media)
        {
            var blogMedia = new BlogMedia
            {
                BlogId = blog.Id,
                MediaType = media.MediaType,
                MediaUrl = media.MediaUrl,
                IsActive = true,
                CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
                LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now)
            };
            
            await _context.BlogMedias.AddAsync(blogMedia, cancellationToken);
        }
        _context.SaveChangesAsync(cancellationToken);
        //end
        
        var result = await _context.Blogs
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.BlogMedias)
            .FirstOrDefaultAsync(x => x.Id == blog.Id, cancellationToken);
        
        return _mapper.Map<BlogDto>(result);
    }
}