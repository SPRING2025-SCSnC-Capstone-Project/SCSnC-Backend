using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Blogs.Queries.GetBlogsPaginated;

public record GetBlogsPaginatedQuery : IRequest<PaginatedList<BlogDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetBlogsPaginatedQueryHandler : IRequestHandler<GetBlogsPaginatedQuery, PaginatedList<BlogDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetBlogsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<BlogDto>> Handle(GetBlogsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Blogs
            .Include(x => x.User)
            .Include(x => x.Event)
            .Include(x => x.BlogMedias)
            //.Select(b => _mapper.Map<BlogDto>(b))
            .AsQueryable();

        return await query.ListPaginateWithSortAsync<Blog, BlogDto>
        (
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}