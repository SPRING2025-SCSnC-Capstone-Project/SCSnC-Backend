namespace Api.Controllers.Payload.Requests;

public class GetWorkspacesPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
    public Guid? BranchId { get; set; }
    public int? SlotNumber { get; set; }
    public DateOnly? ReserveDate { get; set; }
}