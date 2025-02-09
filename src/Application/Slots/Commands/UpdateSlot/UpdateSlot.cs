using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using NodaTime;

namespace Application.Slots.Commands;

public record UpdateSlotCommand: IRequest<SlotDto> {
    public Guid Id { get; init; }
    public int SlotNumber { get; init; }
    public TimeOnly TimeStart { get; init; }
    public TimeOnly TimeEnd { get; init; }
}

public class UpdateSlotCommandHandler: IRequestHandler<UpdateSlotCommand, SlotDto> {
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public UpdateSlotCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SlotDto> Handle(UpdateSlotCommand request, CancellationToken cancellationToken) {
        var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == request.Id  && x.IsActive, cancellationToken);

        if (slot is null) {
            throw new KeyNotFoundException($"Slot with Id {request.Id} does not exist.");
        }

        var existingSlotNumber = await _context.Slots.FirstOrDefaultAsync(x => x.SlotNumber == request.SlotNumber && x.IsActive, cancellationToken);

        if (existingSlotNumber is not null && slot.SlotNumber != existingSlotNumber.SlotNumber) {
            throw new ConflictException($"Slot with number {request.SlotNumber} already exists");
        }

        if (await CheckConflict(request, cancellationToken)) {
            throw new ConflictException($"Conflict detected between slots");
        }

        slot.SlotNumber = request.SlotNumber;
        slot.TimeStart = LocalTime.FromTimeOnly(request.TimeStart);
        slot.TimeEnd = LocalTime.FromTimeOnly(request.TimeEnd);

        _context.Slots.Update(slot);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SlotDto>(slot);
    }

    private async Task<bool> CheckConflict(UpdateSlotCommand request, CancellationToken cancellationToken) {
        var conflict = await _context.Slots.FirstOrDefaultAsync(x => (
            x.SlotNumber > request.SlotNumber && (
                x.TimeStart <= LocalTime.FromTimeOnly(request.TimeStart) || (
                    x.TimeStart <= LocalTime.FromTimeOnly(request.TimeEnd) && x.TimeEnd >= LocalTime.FromTimeOnly(request.TimeEnd)
                )
            )
        ) || (
            x.SlotNumber < request.SlotNumber && (
                x.TimeEnd >= LocalTime.FromTimeOnly(request.TimeEnd) || (
                    x.TimeEnd >= LocalTime.FromTimeOnly(request.TimeStart) && x.TimeStart <= LocalTime.FromTimeOnly(request.TimeStart)
                )
            )
        ), cancellationToken);

        return conflict is not null;
    }
}