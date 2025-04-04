namespace Api.Controllers.Payload.Requests;

public class GetUsersPaginatedRequest : PaginatedQueryParameters {
    public string? SearchTerm { get; set; }
}
