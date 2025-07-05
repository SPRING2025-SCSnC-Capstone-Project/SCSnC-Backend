using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.ShiftSelections.Queries.GetShiftSelectionsPaginatedByEmployeeId;

public record GetShiftSelectionsPaginatedByEmployeeIdQuery : IRequest<PaginatedList<ShiftSelectionDto>>
{
    public Guid EmployeeId { get; set; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public string? Status { get; init; }
}

public class GetShiftSelectionsByEmployeeIdQueryHandler : IRequestHandler<GetShiftSelectionsPaginatedByEmployeeIdQuery, PaginatedList<ShiftSelectionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public async Task<PaginatedList<ShiftSelectionDto>> Handle(GetShiftSelectionsPaginatedByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        if (request.EmployeeId == null)
        {
            throw new ArgumentNullException(nameof(request.EmployeeId), "Employee ID must be provided");
        }

        if (await _context.Users.AnyAsync(x => x.Id == request.EmployeeId) == false)
        {
            throw new KeyNotFoundException($"Employee with ID {request.EmployeeId} not found");
        }

        IQueryable<ShiftSelection> query = new List<ShiftSelection>().AsQueryable();
        
        switch (request.Status)
        {
            case "CHANGED":
                query = _context.ShiftSelections
                    .Include(x => x.ShiftType)
                    .Where(x => x.UserId == request.EmployeeId && x.Status == "CHANGED")
                    .AsQueryable();
                break;
            case "SELECTED":
                query = _context.ShiftSelections
                    .Include(x => x.ShiftType)
                    .Where(x => x.UserId == request.EmployeeId && x.Status == "SELECTED")
                    .AsQueryable();
                break;
            case "CANCELED":
                query = _context.ShiftSelections
                    .Include(x => x.ShiftType)
                    .Where(x => x.UserId == request.EmployeeId && x.Status == "CANCELED")
                    .AsQueryable();
                break;
            default:
                query = _context.ShiftSelections
                    .Include(x => x.ShiftType)
                    .Where(x => x.UserId == request.EmployeeId)
                    .AsQueryable();
                break;
        }
        
        return await query.ListPaginateWithSortAsync<ShiftSelection, ShiftSelectionDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}