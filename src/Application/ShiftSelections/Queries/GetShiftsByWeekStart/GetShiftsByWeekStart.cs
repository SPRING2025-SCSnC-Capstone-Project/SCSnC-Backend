using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.ShiftSelections.Queries.GetShiftsByWeekStart;

public record GetShiftsByWeekStartQuery : IRequest<List<ShiftSummaryDto>> {
    public DateOnly WeekStart { get; init; }
    public Guid BranchId { get; init; }
}

public class GetShiftsByWeekStartQueryHandler : IRequestHandler<GetShiftsByWeekStartQuery, List<ShiftSummaryDto>> {
    private readonly IApplicationDbContext _context;

    public GetShiftsByWeekStartQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
    }

    public async Task<List<ShiftSummaryDto>> Handle(GetShiftsByWeekStartQuery request, CancellationToken cancellationToken) {
        var shifts = await _context.ShiftSelections
            .Where(s => s.Date.ToDateOnly() >= request.WeekStart && s.Date.ToDateOnly() < request.WeekStart.AddDays(7) && s.BranchId == request.BranchId)
            .ToListAsync(cancellationToken);

        var shiftSummaries = shifts.GroupBy(s => new {
            s.Date,
            s.ShiftType.Name
        })
        .Select(g => new ShiftSummaryDto {
            Date = g.Key.Date.ToDateOnly(),
            ShiftTypeName = g.Key.Name,
            Employees = [.. g.Select(s => s.User)]
        })
        .ToList();

        return shiftSummaries;
    }
}
