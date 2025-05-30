namespace Api.Controllers.Payload.Requests.Events;
public class UpdateEventRequest
{
    public string? EventTitle { get; set; } = null!;
    public IFormFile? Image { get; set; }

}
