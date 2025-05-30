namespace Api.Controllers.Payload.Requests.RegistrationWindow;

public class UpdateRegistrationWindowRequest
{
    public DateOnly WeekStart { get; set; }
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
}
