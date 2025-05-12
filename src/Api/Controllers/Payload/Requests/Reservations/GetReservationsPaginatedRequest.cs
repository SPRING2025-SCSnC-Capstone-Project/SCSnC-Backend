namespace Api.Controllers.Payload.Requests;

public class GetReservationsPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
    public bool GetAllReservationByBranch { get; init; } = false;
    public Guid? BranchId { get; init; }
}
