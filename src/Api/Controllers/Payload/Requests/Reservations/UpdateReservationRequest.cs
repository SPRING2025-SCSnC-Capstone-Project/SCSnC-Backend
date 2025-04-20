namespace Api.Controllers.Payload.Requests;

public class UpdateReservationRequest {
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public bool? IsFullPaid { get; set; }
}
