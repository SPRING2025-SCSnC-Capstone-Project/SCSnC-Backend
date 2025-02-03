using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;

namespace Application.Slots.Commands;

public record AddSlotCommand: IRequest<SlotDto> {
    public int SlotNumber { get; init; }
    public TimeOnly TimeStart { get; init; }
    public TimeOnly TimeEnd { get; init; }
}

public class AddSlotCommandHandler: IRequestHandler<AddSlotCommand, SlotDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddSlotCommandHandler(IApplicationDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SlotDto> Handle(AddSlotCommand request, CancellationToken cancellationToken) {
        var slot = await _context.Slots.FirstOrDefaultAsync(x => x.SlotNumber == request.SlotNumber && x.IsActive, cancellationToken);

        if (slot is not null) {
            throw new ConflictException($"Slot with number {request.SlotNumber} already exists");
        }

        if (await CheckConflict(request, cancellationToken)) {
            throw new ConflictException($"Conflict detected between slots");
        }

        var entity = new Slot() {
            SlotNumber = request.SlotNumber,
            IsActive = true,
            TimeStart = LocalTime.FromTimeOnly(request.TimeStart),
            TimeEnd = LocalTime.FromTimeOnly(request.TimeEnd),
        };

        var result = await _context.Slots.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SlotDto>(result.Entity);
    }

    private async Task<bool> CheckConflict(AddSlotCommand request, CancellationToken cancellationToken) {
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