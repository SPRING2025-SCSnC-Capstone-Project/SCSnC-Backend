using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using NodaTime;
using NodaTime.Extensions;

namespace Application.ShiftSelections.Commands.RegisterShift;

public record DateWithShiftTypeId
{
    public DateOnly Date { get; init; }
    public Guid ShiftTypeId { get; init; }
}

public record DateWithShiftType
{
    public DateOnly Date { get; init; }
    public ShiftType ShiftType { get; init; } = null!;
}

public record RegisterShiftCommand : IRequest<List<ShiftSelectionDto>>
{
    public Guid UserId { get; init; }
    public Guid BranchId { get; init; }
    public DateOnly WeekStart { get; init; }
    public List<DateWithShiftTypeId> DatesWithShiftTypeIds { get; init; } = [];
}

public class RegisterShiftCommandHandler : IRequestHandler<RegisterShiftCommand, List<ShiftSelectionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegisterShiftCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ShiftSelectionDto>> Handle(RegisterShiftCommand request, CancellationToken cancellationToken)
    {
        if (request.DatesWithShiftTypeIds.Count < 6)
        {
            throw new RequestValidationException("At least 6 dates with shift type are required");
        }

        var registrationWindow = await _context.RegistrationWindows.FirstOrDefaultAsync(rw => rw.BranchId == request.BranchId, cancellationToken);

        if (registrationWindow is null)
        {
            throw new KeyNotFoundException($"Registration window with branch ID {request.BranchId} not found");
        }
        
        if (LocalDateTime.FromDateTime(DateTime.Now) < registrationWindow.OpenAt || LocalDateTime.FromDateTime(DateTime.Now) > registrationWindow.CloseAt)
        {
            throw new RequestValidationException("Registration window is not open");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId && u.IsActive, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found");
        }

        var branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == request.BranchId && b.IsActive, cancellationToken);

        if (branch is null)
        {
            throw new KeyNotFoundException($"Branch with ID {request.BranchId} not found");
        }

        var datesWithShiftTypes = new List<DateWithShiftType>();
        var shiftSelections = new List<ShiftSelection>();

        foreach (var d in request.DatesWithShiftTypeIds)
        {
            var shiftType = await _context.ShiftTypes.FirstOrDefaultAsync(st => st.Id == d.ShiftTypeId && st.BranchId == request.BranchId, cancellationToken);

            if (shiftType is null)
            {
                throw new KeyNotFoundException($"Shift type with ID {d.ShiftTypeId} in branch with ID {request.BranchId} not found");
            }

            datesWithShiftTypes.Add(new DateWithShiftType { Date = d.Date, ShiftType = shiftType });
        }

        foreach (var d in datesWithShiftTypes)
        {
            var amountOfEmployeesInSameShift = await _context.ShiftSelections.CountAsync(s => s.BranchId == request.BranchId 
                && s.Date == d.Date.ToLocalDate() 
                && s.ShiftTypeId == d.ShiftType.Id, cancellationToken);

            if (amountOfEmployeesInSameShift >= 3)
            {
                throw new RequestValidationException($"At most only 3 employees can be in the same shift on {d.Date}");
            }

            if (d.Date < registrationWindow.WeekStart.ToDateOnly() || d.Date > registrationWindow.WeekStart.ToDateOnly().AddDays(6))
            {
                throw new RequestValidationException($"Shift on {d.Date} is not within the registrated week");
            }

            if (d.ShiftType.Name == "Morning") {
                var consecutiveShift = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && c.ShiftType.Name == "Afternoon");

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both morning and afternoon shift on {consecutiveShift.Date}");
                }

                var conflict = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && c.ShiftType.Name == "Sáng");

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict morning shift on {conflict.Date}");
                }
            } else if (d.ShiftType.Name == "Afternoon") {
                var consecutiveShift = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && (c.ShiftType.Name == "Evening" 
                    || c.Date == d.Date && c.ShiftType.Name == "Morning"));

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both afternoon and evening/morning shift on {consecutiveShift.Date}");
                }

                var conflict = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && c.ShiftType.Name == "Afternoon");

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict afternoon shift on {conflict.Date}");
                }
            } else if (d.ShiftType.Name == "Evening") {
                var consecutiveShift = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && c.ShiftType.Name == "Afternoon");

                if (consecutiveShift is not null)
                {
                    throw new RequestValidationException($"Cannot have both evening and afternoon shift on {consecutiveShift.Date}");
                }

                var conflict = datesWithShiftTypes.FirstOrDefault(c => c.Date == d.Date && c.ShiftType.Name == "Evening");

                if (conflict is not null)
                {
                    throw new RequestValidationException($"Conflict evening shift on {conflict.Date}");
                }
            }

            shiftSelections.Add(new ShiftSelection
            {
                BranchId = request.BranchId,
                Date = d.Date.ToLocalDate(),
                ShiftTypeId = d.ShiftType.Id,
                UserId = request.UserId,
                WeekStart = request.WeekStart.ToLocalDate(),
                Status = "Selected"
            });
        }

        await _context.ShiftSelections.AddRangeAsync(shiftSelections, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.ShiftSelections.Where(s => s.UserId == request.UserId
            && s.BranchId == request.BranchId
            && s.WeekStart == request.WeekStart.ToLocalDate()).ToListAsync(cancellationToken);

        return _mapper.Map<List<ShiftSelectionDto>>(result);
    }
}
