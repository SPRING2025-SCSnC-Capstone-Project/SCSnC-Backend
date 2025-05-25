namespace Api.Controllers.Payload.Requests.ShiftTypes;

public class AddShiftTypeRequest
{
    public Guid BranchId { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
}