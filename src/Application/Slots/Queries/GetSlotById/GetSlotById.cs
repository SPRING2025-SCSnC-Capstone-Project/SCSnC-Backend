using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Slots.Queries;

public record GetSlotByIdQuery : IRequest<SlotDto> {
    public Guid Id { get; init; }
}

public class GetSlotByIdQueryHandler : IRequestHandler<GetSlotByIdQuery, SlotDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetSlotByIdQueryHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SlotDto> Handle(GetSlotByIdQuery request, CancellationToken cancellationToken) {
        var table = await _context.Slots.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (table is null) {
            throw new KeyNotFoundException($"Slot with Id {request.Id} does not exist");
        }

        return _mapper.Map<SlotDto>(table);
    }
}