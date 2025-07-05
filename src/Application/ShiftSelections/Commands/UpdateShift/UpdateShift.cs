using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime.Extensions;

namespace Application.ShiftSelections.Commands.UpdateShift;

public record UpdateShiftCommand : IRequest<List<ShiftSelectionDto>>
{
    public Guid UserId { get; init; }
    public List<ShiftSelectionUpdateWithId> ShiftSelectionUpdatesWithId { get; init; } = [];
}

public record ShiftSelectionUpdateWithId
{
    public Guid ShiftSelectionId { get; init; }
    public DateOnly NewDate { get; init; }
    public Guid NewShiftTypeId { get; init; }
}

public record ShiftSelectionUpdateWithoutId
{
    public ShiftSelection ShiftSelection { get; init; } = null!;
    public DateOnly NewDate { get; init; }
    public ShiftType NewShiftType { get; init; } = null!;
}

public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, List<ShiftSelectionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateShiftCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ShiftSelectionDto>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        var shiftSelectionUpdateWithoutIds = new List<ShiftSelectionUpdateWithoutId>();
        var shiftsToUpdate = new List<ShiftSelection>();
        foreach (var shiftSelectionUpdate in request.ShiftSelectionUpdatesWithId)
        {
            var shiftSelection = await _context.ShiftSelections.FirstOrDefaultAsync(s => s.Id == shiftSelectionUpdate.ShiftSelectionId, cancellationToken);
            if (shiftSelection is null)
            {
                throw new KeyNotFoundException($"Shift selection with id {shiftSelectionUpdate.ShiftSelectionId} not found");
            }

            var newShiftType = await _context.ShiftTypes.FirstOrDefaultAsync(s => s.Id == shiftSelectionUpdate.NewShiftTypeId, cancellationToken);
            if (newShiftType is null)
            {
                throw new KeyNotFoundException($"Shift type with id {shiftSelectionUpdate.NewShiftTypeId} not found");
            }

            shiftSelectionUpdateWithoutIds.Add(new ShiftSelectionUpdateWithoutId
            {
                ShiftSelection = shiftSelection,
                NewDate = shiftSelectionUpdate.NewDate,
                NewShiftType = newShiftType
            });
        }

        foreach (var shiftSelectionUpdate in shiftSelectionUpdateWithoutIds) {
            var amountOfEmployeesInSameShift = await _context.ShiftSelections.CountAsync(s => s.BranchId == shiftSelectionUpdate.ShiftSelection.BranchId 
                && s.Date == shiftSelectionUpdate.NewDate.ToLocalDate() 
                && s.ShiftTypeId == shiftSelectionUpdate.NewShiftType.Id, cancellationToken);

            if (amountOfEmployeesInSameShift >= 3)
            {
                throw new RequestValidationException($"At most only 3 employees can be in the same shift on {shiftSelectionUpdate.NewDate}");
            }

            if (shiftSelectionUpdate.ShiftSelection.Date.ToDateOnly().DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber < 3) {
                throw new RequestValidationException($"Not allowed to update shift when difference between current date and shift date is less than 3 days");
            }

            if (shiftSelectionUpdate.NewDate < shiftSelectionUpdate.ShiftSelection.WeekStart.ToDateOnly() 
                || shiftSelectionUpdate.NewDate > shiftSelectionUpdate.ShiftSelection.WeekStart.ToDateOnly().AddDays(6))
            {
                throw new RequestValidationException($"Shift on {shiftSelectionUpdate.NewDate} is not within the registrated week");
            }

            if (shiftSelectionUpdate.NewShiftType.Name == "Morning") {
                var consecutiveShift = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && c.ShiftType.Name == "Afternoon" 
                    && c.UserId == shiftSelectionUpdate.ShiftSelection.UserId, cancellationToken);

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both morning and afternoon shift on {consecutiveShift.Date}");
                }

                var conflict = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && c.ShiftType.Name == "Morning", cancellationToken);

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict morning shift on {conflict.Date}");
                }
            } else if (shiftSelectionUpdate.NewShiftType.Name == "Afternoon") {
                var consecutiveShift = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && (c.ShiftType.Name == "Evening" 
                    || c.ShiftType.Name == "Morning") && c.UserId == shiftSelectionUpdate.ShiftSelection.UserId, cancellationToken);

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both afternoon and evening/morning shift on {consecutiveShift.Date}");
                }

                var conflict = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && c.ShiftType.Name == "Afternoon", cancellationToken);

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict afternoon shift on {conflict.Date}");
                }
            } else if (shiftSelectionUpdate.NewShiftType.Name == "Evening") {
                var consecutiveShift = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && c.ShiftType.Name == "Afternoon"
                    && c.UserId == shiftSelectionUpdate.ShiftSelection.UserId, cancellationToken);

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both evening and afternoon shift on {consecutiveShift.Date}");
                }

                var conflict = await _context.ShiftSelections
                .FirstOrDefaultAsync(c => c.Date.ToDateOnly() == shiftSelectionUpdate.NewDate && c.ShiftType.Name == "Evening", cancellationToken);

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict evening shift on {conflict.Date}");
                }
            }

            shiftSelectionUpdate.ShiftSelection.Date = shiftSelectionUpdate.NewDate.ToLocalDate();
            shiftSelectionUpdate.ShiftSelection.ShiftTypeId = shiftSelectionUpdate.NewShiftType.Id;
            shiftsToUpdate.Add(shiftSelectionUpdate.ShiftSelection);
        }

        _context.ShiftSelections.UpdateRange(shiftsToUpdate);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<List<ShiftSelectionDto>>(shiftsToUpdate);
    }
}