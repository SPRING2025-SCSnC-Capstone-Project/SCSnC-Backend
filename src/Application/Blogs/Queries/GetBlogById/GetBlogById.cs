using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Blogs.Queries.GetBlogById;

public record GetBlogByIdQuery : IRequest<BlogDto>
{
    public Guid Id { get; set; }
}

public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, BlogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetBlogByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Blogs
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.BlogMedias)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        return _mapper.Map<BlogDto>(result);
    }
}