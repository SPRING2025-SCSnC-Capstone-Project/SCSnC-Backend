using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Tables.Queries;

public record GetTablesPaginatedQuery: IRequest<PaginatedList<TableDto>> {
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }

}

public class GetTablesPaginatedQueryHandler: IRequestHandler<GetTablesPaginatedQuery, PaginatedList<TableDto>> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetTablesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TableDto>> Handle(GetTablesPaginatedQuery request, CancellationToken cancellationToken) {
        var tables = _context.Tables.AsQueryable().Where(x => x.IsActive);

        return await tables.ListPaginateWithSortAsync<Table, TableDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}