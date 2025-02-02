using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Tables.Commands;

public record UpdateTableCommand: IRequest<TableDto> {
    public Guid Id { get; init; }
    public int TableNumber { get; init; }
    public int SeatAmount { get; init; }
}

public class UpdateTableCommandHandler: IRequestHandler<UpdateTableCommand, TableDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public UpdateTableCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TableDto> Handle(UpdateTableCommand request, CancellationToken cancellationToken) {
        var table = await _context.Tables.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (table is null) {
            throw new KeyNotFoundException($"Table with Id {request.Id} does not exist.");
        }

        var existingTableNumber = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == request.TableNumber, cancellationToken);

        if (existingTableNumber is not null && table.TableNumber != existingTableNumber.TableNumber) {
            throw new ConflictException($"Table with number {request.TableNumber} already exists");
        }

        table.TableNumber = request.TableNumber;
        table.SeatAmount = request.SeatAmount;
        table.LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now);

        _context.Tables.Update(table);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TableDto>(table);
    }
}