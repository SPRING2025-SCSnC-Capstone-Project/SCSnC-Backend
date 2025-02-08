using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Toppings.Queries.GetToppingsPaginated;

public record GetToppingsPaginatedQuery : IRequest<PaginatedList<ToppingDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetToppingsPaginatedQueryHandler : IRequestHandler<GetToppingsPaginatedQuery, PaginatedList<ToppingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetToppingsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ToppingDto>> Handle(GetToppingsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Toppings.AsQueryable();
        
        return await query.ListPaginateWithSortAsync<Topping, ToppingDto>(
            request.Page, 
            request.Size, 
            request.SortBy, 
            request.SortOrder, 
            _mapper.ConfigurationProvider, 
            cancellationToken
        );
    }
}