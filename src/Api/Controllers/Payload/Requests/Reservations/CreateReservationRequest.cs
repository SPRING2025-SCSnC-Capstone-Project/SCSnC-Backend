namespace Api.Controllers.Payload.Requests;

public class CreateReservationRequest {
    public DateOnly ReservationDate { get; set; }
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public Guid UserId { get; set; }
    public double TotalPrice { get; set; }
} 
