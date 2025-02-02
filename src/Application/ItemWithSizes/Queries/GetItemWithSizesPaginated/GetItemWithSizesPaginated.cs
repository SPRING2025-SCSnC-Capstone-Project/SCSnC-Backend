using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.ItemWithSizes.Queries.GetItemWithSizesPaginated;

public record GetItemWithSizesPaginated : IRequest<PaginatedList<ItemWithSizeDto>>
{
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetItemWithSizesPaginatedHandler : IRequestHandler<GetItemWithSizesPaginated, PaginatedList<ItemWithSizeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetItemWithSizesPaginatedHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ItemWithSizeDto>> Handle(GetItemWithSizesPaginated request, CancellationToken cancellationToken)
    {
        var query = _context.ItemWithSizes
            .Include(x => x.Item)
            .Include(x => x.Size)
            .AsQueryable();

        return await query
            .ListPaginateWithSortAsync<ItemWithSize, ItemWithSizeDto>(
                request.Page,
                request.Size,
                request.SortBy,
                request.SortOrder,
                _mapper.ConfigurationProvider,
                cancellationToken
            );
    }
}