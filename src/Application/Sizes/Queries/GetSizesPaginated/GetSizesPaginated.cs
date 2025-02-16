using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Sizes.Queries.GetSizesPaginated;

public record GetSizesPaginatedQuery : IRequest<PaginatedList<SizeDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetSizesPaginatedQueryHandler : IRequestHandler<GetSizesPaginatedQuery, PaginatedList<SizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetSizesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<SizeDto>> Handle(GetSizesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Sizes.AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Size, SizeDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}