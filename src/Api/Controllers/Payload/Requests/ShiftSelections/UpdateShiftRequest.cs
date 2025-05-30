using Application.ShiftSelections.Commands.UpdateShift;

namespace Api.Controllers.Payload.Requests.ShiftSelections;

public class UpdateShiftRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<ShiftSelectionUpdateWithId> ShiftSelectionUpdatesWithId { get; set; } = [];
}

