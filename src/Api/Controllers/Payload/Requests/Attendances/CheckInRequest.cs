namespace Api.Controllers.Payload.Requests.Attendances;

public class CheckInRequest {
    public Guid BranchId { get; set; }
    public Guid ShiftTypeId { get; set; }
    public Guid EmployeeId { get; set; }
}
