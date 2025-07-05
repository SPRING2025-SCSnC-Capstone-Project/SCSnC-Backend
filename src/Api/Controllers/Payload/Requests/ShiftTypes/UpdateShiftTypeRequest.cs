namespace Api.Controllers.Payload.Requests.ShiftTypes;

public class UpdateShiftTypeRequest {
    public Guid BranchId { get; set; }
    public string Name { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
