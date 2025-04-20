namespace Api.Controllers.Payload.Requests;

public class UpdateTableRequest {
    public int TableNumber { get; set; }
    public int SeatAmount { get; set; }
    public Guid BranchId { get; set; }
    public bool IsAvailable { get; set; }
}
