using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Tables.Commands;

public record RemoveTableCommand: IRequest<TableDto> {
    public Guid Id { get; init; }
}

public class RemoveTableCommandHandler: IRequestHandler<RemoveTableCommand, TableDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RemoveTableCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TableDto> Handle(RemoveTableCommand request, CancellationToken cancellationToken) {
        var table = await _context.Tables.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (table is null) {
            throw new KeyNotFoundException($"Table with Id {request.Id} does not exist");
        }

        table.IsActive = false;

        _context.Tables.Update(table);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TableDto>(table);
    }
}
