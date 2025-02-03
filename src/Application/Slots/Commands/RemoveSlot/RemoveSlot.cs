using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Slots.Commands;

public record RemoveSlotCommand: IRequest<SlotDto> {
    public Guid Id { get; init; }
}

public class RemoveSlotCommandHandler: IRequestHandler<RemoveSlotCommand, SlotDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public RemoveSlotCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SlotDto> Handle(RemoveSlotCommand request, CancellationToken cancellationToken) {
        var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (slot is null) {
            throw new KeyNotFoundException($"Slot with Id {request.Id} does not exist");
        }

        slot.IsActive = false;

        _context.Slots.Update(slot);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SlotDto>(slot);
    }
}