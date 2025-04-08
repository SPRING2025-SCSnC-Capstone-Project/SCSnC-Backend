using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;

namespace Application.Reservations.Commands;

public record CreateReservationCommand : IRequest<ReservationDto> {
    public DateOnly ReservationDate { get; init; }
    public Guid WorkspaceId { get; init; }
    public double Deposit { get; init; }
    public Guid UserId { get; init; }
    public double TotalPrice { get; init; }
    public Guid[] SlotIds { get; init; } = null!;
}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReservationCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken) {
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(x => x.Id == request.WorkspaceId && x.IsActive, cancellationToken);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.IsActive, cancellationToken);

        if (workspace is null) {
            throw new KeyNotFoundException($"Workspace with Id {request.WorkspaceId} does not exist");
        }

        if (user is null) {
            throw new KeyNotFoundException($"User with Id {request.UserId} does not exist");
        }

        var reservedSlotsToAdd = new List<ReservedSlot>(); 

        foreach (var slotId in request.SlotIds) {
            var slot = await _context.Slots.FirstOrDefaultAsync(x => x.Id == slotId && x.IsActive, cancellationToken);
            
            if (slot is null) {
                throw new KeyNotFoundException($"Slot with Id {slotId} does not exist");
            }

            if (await CheckConflict(request.ReservationDate, slotId, cancellationToken)) {
                throw new ConflictException("One or more slots have already been reserved");
            }
        }

        var reservation = new Reservation() {
            UserId = request.UserId,
            WorkspaceId = request.WorkspaceId,
            Deposit = request.Deposit,
            IsFullPaid = false,
            TotalPrice = request.TotalPrice,
            ReserveDate = LocalDate.FromDateOnly(request.ReservationDate)
        };

        var result = await _context.Reservations.AddAsync(reservation, cancellationToken);

        foreach (var slotId in request.SlotIds) {
            var reservedSlot = new ReservedSlot() {
                SlotId = slotId,
                ReservationId = result.Entity.Id,
            };
            
            reservedSlotsToAdd.Add(reservedSlot);
        };

        await _context.ReservedSlots.AddRangeAsync(reservedSlotsToAdd, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var createdReservation = await _context.Reservations
            .Include(x => x.Workspace)
            .ThenInclude(y => y.WorkspaceType)
            .Include(x => x.User)
            .Include(x => x.ReservedSlots)
            .ThenInclude(y => y.Slot)
            .FirstOrDefaultAsync(x => x.Id == result.Entity.Id, cancellationToken);

        return _mapper.Map<ReservationDto>(createdReservation);
    }

    private async Task<bool> CheckConflict(DateOnly reservationDate, Guid slotId, CancellationToken cancellationToken) {
        var conflict = await _context.ReservedSlots.Include(x => x.Reservation).FirstOrDefaultAsync(x => 
                x.Reservation!.ReserveDate == LocalDate.FromDateOnly(reservationDate) && 
                x.SlotId == slotId, cancellationToken);

        return conflict is not null;
    }
}
