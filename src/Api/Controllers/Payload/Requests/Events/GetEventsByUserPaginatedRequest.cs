namespace Api.Controllers.Payload.Requests;

public class GetEventsPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; } 
}
