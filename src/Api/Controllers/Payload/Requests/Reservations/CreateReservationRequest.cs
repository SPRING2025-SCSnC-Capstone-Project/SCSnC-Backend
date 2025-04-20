namespace Api.Controllers.Payload.Requests;

public class CreateReservationRequest {
    public DateOnly ReservationDate { get; set; }
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    public Guid? UserId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public double TotalPrice { get; set; }
    public Guid[] SlotIds { get; set; }
    public Guid[]? WorkspaceUtilityServiceIds { get; set; }
    public string PaymentMethod { get; set; }
} 
