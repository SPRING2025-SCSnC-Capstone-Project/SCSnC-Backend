using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Slots.Queries;

public record GetSlotsPaginatedQuery: IRequest<PaginatedList<SlotDto>> {
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }

}

public class GetSlotsPaginatedQueryHandler: IRequestHandler<GetSlotsPaginatedQuery, PaginatedList<SlotDto>> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetSlotsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SlotDto>> Handle(GetSlotsPaginatedQuery request, CancellationToken cancellationToken) {
        var tables = _context.Slots.AsQueryable().Where(x => x.IsActive);

        return await tables.ListPaginateWithSortAsync<Slot, SlotDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}