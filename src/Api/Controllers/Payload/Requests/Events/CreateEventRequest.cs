namespace Api.Controllers.Payload.Requests;

public class CreateEventRequest {
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public string? CoverImageLink { get; set; }
    public double EntranceFee { get; set; }
    public TimeOnly EventStartTime { get; set; }
    public TimeOnly EventEndTime { get; set; }
    public Guid ReservationId { get; set; }
    public Guid CurrentUserId { get; set; }
}
