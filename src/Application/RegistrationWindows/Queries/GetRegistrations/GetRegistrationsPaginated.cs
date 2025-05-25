using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.RegistrationWindows.Queries.GetRegistrations;

public record GetRegistrationsPaginatedQuery : IRequest<PaginatedList<RegistrationWindowDto>>
{
    public Guid BranchId { get; init; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
}

public class GetRegistrationsPaginatedQueryHandler : IRequestHandler<GetRegistrationsPaginatedQuery, PaginatedList<RegistrationWindowDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRegistrationsPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RegistrationWindowDto>> Handle(GetRegistrationsPaginatedQuery request, CancellationToken cancellationToken)
    {
        if (request.BranchId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(request.BranchId), "Branch ID must be provided");
        }

        var query = _context.RegistrationWindows
            .Include(x => x.Branch)
            .Where(x => x.BranchId == request.BranchId)
            .AsQueryable();

        return await query.ListPaginateWithSortAsync<RegistrationWindow, RegistrationWindowDto>(
            request.Page,
            request.Size,
            request.SortBy,
            request.SortOrder,
            _mapper.ConfigurationProvider,
            cancellationToken
        );
    }
}