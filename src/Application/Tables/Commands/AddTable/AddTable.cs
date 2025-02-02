using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Tables.Commands;

public record AddTableCommand: IRequest<TableDto> {
    public int TableNumber { get; init; }
    public int SeatAmount { get; init; }
}

public class AddTableCommandHandler: IRequestHandler<AddTableCommand, TableDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddTableCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TableDto> Handle(AddTableCommand request, CancellationToken cancellationToken) {
        var table = await _context.Tables.FirstOrDefaultAsync(x => x.TableNumber == request.TableNumber, cancellationToken);

        if (table is not null) {
            throw new ConflictException($"Table with number {request.TableNumber} already exists");
        }

        var entity = new Table() {
            TableNumber = request.TableNumber,
            SeatAmount = request.SeatAmount,
            IsAvailable = true,
            CreatedAt = LocalDateTime.FromDateTime(DateTime.Now),
            LastUpdatedAt = LocalDateTime.FromDateTime(DateTime.Now),
        };

        var result = await _context.Tables.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TableDto>(result.Entity);
    }
}