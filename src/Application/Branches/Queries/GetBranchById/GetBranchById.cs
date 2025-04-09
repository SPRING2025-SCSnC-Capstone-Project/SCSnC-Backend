using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Branches.Queries.GetBranchById;

public record GetBranchByIdQuery : IRequest<BranchDto>
{
    public Guid Id { get; init; }
}

public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, BranchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBranchByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BranchDto> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (branch is null)
        {
            throw new KeyNotFoundException($"Branch with id {request.Id} not found");
        }

        return _mapper.Map<BranchDto>(branch);
    }
}