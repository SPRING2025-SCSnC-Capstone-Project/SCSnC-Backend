namespace Api.Controllers.Payload.Requests;

public class GetEventsPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
    public bool GetAllEvent {  get; set; }
    public Guid? BranchId { get; set; }
}
