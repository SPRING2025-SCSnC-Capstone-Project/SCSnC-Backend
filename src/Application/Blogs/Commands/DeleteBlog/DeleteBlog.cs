using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Blogs.Commands.DeleteBlog;

public record DeleteBlogCommand : IRequest<BlogDto>
{
    public Guid BlogId { get; init; }
}

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, BlogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public DeleteBlogCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BlogDto> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _context.Blogs
            .FirstOrDefaultAsync(x => x.Id == request.BlogId, cancellationToken);
        var blogMedias = await _context.BlogMedias
            .Where(x => x.BlogId == request.BlogId)
            .ToListAsync(cancellationToken);

        blog.IsActive = false;
        blog.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
        
        _context.Blogs.Update(blog);
        _context.SaveChangesAsync(cancellationToken);
        
        foreach (var blogMedia in blogMedias)
        {
            blogMedia.IsActive = false;
            blogMedia.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);
            
            _context.BlogMedias.Update(blogMedia);
        }
        _context.SaveChangesAsync(cancellationToken);
        
        var result = await _context.Blogs
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.BlogMedias)
            .FirstOrDefaultAsync(x => x.Id == blog.Id, cancellationToken);
        
        return _mapper.Map<BlogDto>(result);
    }
}