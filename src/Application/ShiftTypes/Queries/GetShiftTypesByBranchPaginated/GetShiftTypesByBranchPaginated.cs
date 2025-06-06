using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.ShiftTypes.Queries;

public record GetShiftTypesByBranchQuery : IRequest<PaginatedList<ShiftTypeDto>>
{
    public Guid BranchId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetShiftTypesByBranchQueryHandler : IRequestHandler<GetShiftTypesByBranchQuery, PaginatedList<ShiftTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShiftTypesByBranchQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ShiftTypeDto>> Handle(GetShiftTypesByBranchQuery request, CancellationToken cancellationToken)
    {
        var shiftTypes = _context.ShiftTypes
            .AsQueryable()
            .Where(x => x.BranchId == request.BranchId && x.IsActive);

        return await shiftTypes.ListPaginateWithSortAsync<ShiftType, ShiftTypeDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}