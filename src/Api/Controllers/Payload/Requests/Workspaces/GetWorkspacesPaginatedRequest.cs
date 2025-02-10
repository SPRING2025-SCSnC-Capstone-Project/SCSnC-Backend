namespace Api.Controllers.Payload.Requests;

public class GetWorkspacesPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
}