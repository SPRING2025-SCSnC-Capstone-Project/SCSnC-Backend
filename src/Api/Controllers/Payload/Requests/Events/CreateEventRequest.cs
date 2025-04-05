namespace Api.Controllers.Payload.Requests;

public class CreateEventRequest {
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string? CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public Guid WorkspaceId { get; set; }
    public Guid UserId { get; set; }
}
