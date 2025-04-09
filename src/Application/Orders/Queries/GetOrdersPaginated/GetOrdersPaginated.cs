using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries.GetOrdersPaginated;

public record GetOrdersPaginatedQuery : IRequest<PaginatedList<OrderDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetOrdersPaginatedQueryHandler : IRequestHandler<GetOrdersPaginatedQuery, PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetOrdersPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<OrderDto>> Handle(GetOrdersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders
            .Include(x => x.Table)
            .Include(x => x.Voucher)
            .Include(x => x.User)
            .Include(x => x.Branch)
            .AsQueryable();

        return await query.ListPaginateWithSortAsync<Order, OrderDto>
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