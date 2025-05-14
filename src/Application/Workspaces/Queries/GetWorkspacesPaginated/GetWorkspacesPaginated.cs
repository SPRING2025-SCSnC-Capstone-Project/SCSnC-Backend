using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Domain.Entities;
using System.Diagnostics;
using NodaTime;

namespace Application.Workspaces.Queries;

public record GetWorkspacesPaginatedQuery : IRequest<PaginatedList<WorkspaceDto>>
{
    public string? Filter { get; set; }
    public int? Page { get; init; }
    public int? Size { get; init; }
    public string? SortBy { get; init; }
    public string? SortOrder { get; init; }
    public int? SlotNumber { get; set; }
    public DateOnly? ReserveDate { get; set; }
}

public class GetWorkspacesPaginatedQueryHandler : IRequestHandler<GetWorkspacesPaginatedQuery, PaginatedList<WorkspaceDto>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetWorkspacesPaginatedQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WorkspaceDto>> Handle(GetWorkspacesPaginatedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workspaces = _context.Workspaces.Include(x => x.WorkspaceTypeAtBranch.WorkspaceType)
                .Where(x => x.IsActive)
                .AsQueryable();

            if (request.BranchId != Guid.Empty && request.BranchId != null)
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.IsActive && x.Id == request.BranchId, cancellationToken);

                if (branch is null)
                {
                    throw new KeyNotFoundException($"Branch with Id {request.BranchId} does not exist");
                }

                workspaces = workspaces.Where(x => x.Branch.Id == branch.Id);
            }

            if (request.WorkspaceTypeId != Guid.Empty && request.WorkspaceTypeId != null)
            {
                var workspaceType = await _context.WorkspaceTypes.FirstOrDefaultAsync(x => x.IsActive && x.Id == request.WorkspaceTypeId, cancellationToken);

                if (workspaceType is null)
                {
                    throw new KeyNotFoundException($"Workspace Type with Id {request.WorkspaceTypeId} does not exist");
                }

                workspaces = workspaces.Where(x => x.WorkspaceType.Id == workspaceType.Id);
            }

            if ((request.ReserveDate != null && request.SlotNumber == null) || (request.ReserveDate == null && request.SlotNumber != null))
            {
                throw new RequestValidationException("Reserve date and Slot number must be both filled in or both empty");
            }

            if (request.SlotNumber != null && request.ReserveDate != null)
            {
                var slot = await _context.Slots.FirstOrDefaultAsync(x => x.IsActive && x.SlotNumber == request.SlotNumber, cancellationToken);

                if (slot is null)
                {
                    throw new KeyNotFoundException($"Slot with Number {request.SlotNumber} does not exist");
                }

                workspaces = workspaces.Where(x => !(x.Reservations.Any(y => y.ReservedSlots.Any(z => z.Slot!.SlotNumber == request.SlotNumber))
                    && x.Reservations.Any(y => y.ReserveDate == LocalDate.FromDateOnly(request.ReserveDate.Value))));
            }

            if (request.Filter != null && !request.Filter.Equals(string.Empty))
            {
                workspaces = workspaces.Where(x => x.WorkspaceTypeAtBranch.WorkspaceType.WorkspaceTypeName == request.Filter);
            }

            return await workspaces.ListPaginateWithSortAsync<Workspace, WorkspaceDto>(
                request.Page,
                request.Size,
                request.SortBy,
                request.SortOrder,
                _mapper.ConfigurationProvider,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw new Exception(ex.Message);
        }

    }
}