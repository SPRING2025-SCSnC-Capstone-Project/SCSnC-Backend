using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries.GetOrdersByBranchPaginated;

public record GetOrdersByBranchPaginatedQuery : IRequest<PaginatedList<ResponseOrderDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public Guid BranchId { get; init; }
}

public class GetOrdersByBranchPaginatedQueryHandler : IRequestHandler<GetOrdersByBranchPaginatedQuery, PaginatedList<ResponseOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetOrdersByBranchPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ResponseOrderDto>> Handle(GetOrdersByBranchPaginatedQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders
            .Include(x => x.Workspace)
                .ThenInclude(y => y.Reservations)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch.WorkspaceType)
            .Include(x => x.Workspace)
                .ThenInclude(y => y.WorkspaceTypeAtBranch.Branch)
            .AsNoTracking()
            .Include(x => x.OrderDetails)
                .ThenInclude(y => y.IncludeToppings)
                    .ThenInclude(z => z.Topping)
            .Include(x => x.OrderDetails)
                .ThenInclude(y => y.ItemWithSize)
                    .ThenInclude(z => z.Item)
            .Include(x => x.OrderDetails)
                .ThenInclude(y => y.ItemWithSize)
                    .ThenInclude(z => z.Size)
            .AsNoTracking()
            .Include(x => x.Transactions)
                .ThenInclude(y => y.Reservation)
            .Include(x => x.Table)
            .Include(x => x.Voucher)
            .Include(x => x.User)
            .Where(x => x.BranchId.Equals(request.BranchId))
            .AsQueryable();

        return await query.ListPaginateWithSortAsync<Order, ResponseOrderDto>
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