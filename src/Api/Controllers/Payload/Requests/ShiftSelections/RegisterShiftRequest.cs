using Application.ShiftSelections.Commands.RegisterShift;

namespace Api.Controllers.Payload.Requests.ShiftSelections;

public class RegisterShiftRequest
{
    public Guid BranchId { get; set; }
    public DateOnly WeekStart { get; set; }
    public List<DateWithShiftTypeId> DatesWithShiftTypeIds { get; set; } = [];
}