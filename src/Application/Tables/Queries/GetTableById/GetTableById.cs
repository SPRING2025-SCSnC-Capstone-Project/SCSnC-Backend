using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Tables.Queries;

public record GetTableByIdQuery : IRequest<TableDto> {
    public Guid Id { get; init; }
}

public class GetTableByIdQueryHandler : IRequestHandler<GetTableByIdQuery, TableDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetTableByIdQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TableDto> Handle(GetTableByIdQuery request, CancellationToken cancellationToken) {
        var table = await _context.Tables
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (table is null) {
            throw new KeyNotFoundException($"Table with Id {request.Id} does not exist");
        }

        return _mapper.Map<TableDto>(table);
    }
}