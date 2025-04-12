namespace Api.Controllers.Payload.Requests;

public class CreateReservationRequest {
    public DateOnly ReservationDate { get; set; }
    public Guid WorkspaceTypeId { get; set; }
    public Guid WorkspaceId { get; set; }
    public double Deposit { get; set; }
    public Guid UserId { get; set; }
    public double TotalPrice { get; set; }
    public string? Note { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    //public DateTimeOffset startDate { get; set; }
    //public DateTimeOffset endDate { get; set; }
    public bool includeEvent { get; set; }
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public string? CoverImageLink { get; set; }
    public double? EntranceFee { get; set; }
    public Guid[] SlotIds { get; set; }
} 
