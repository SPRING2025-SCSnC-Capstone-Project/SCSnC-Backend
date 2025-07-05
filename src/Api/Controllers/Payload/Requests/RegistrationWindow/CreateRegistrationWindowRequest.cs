namespace Api.Controllers.Payload.Requests;

public class CreateRegistrationWindowRequest
{
    public DateOnly WeekStart { get; set; }
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
    public Guid BranchId { get; set; }
}
