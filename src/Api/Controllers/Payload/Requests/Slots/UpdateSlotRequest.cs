namespace Api.Controllers.Payload.Requests;

public class UpdateSlotRequest {
    public int SlotNumber { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
}