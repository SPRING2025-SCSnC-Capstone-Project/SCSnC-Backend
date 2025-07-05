namespace Api.Controllers.Payload.Requests;

public class AddTableRequest {
    public int TableNumber { get; set; }
    public int SeatAmount { get; set; }
    public Guid BranchId { get; set; }
}
