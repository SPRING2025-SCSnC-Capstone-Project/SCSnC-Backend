namespace Api.Controllers.Payload.Requests;

public class GetWorkspacesByTimeAndTypePaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
    public Guid WorkspaceTypes { get; set; }
    public DateOnly ReserveDate { get; set; }
    public Guid[] SlotIds { get; set; }
    public Guid BranchId { get; set; }
    public bool BookingWithTime { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
}