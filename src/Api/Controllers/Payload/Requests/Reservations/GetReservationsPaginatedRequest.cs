namespace Api.Controllers.Payload.Requests;

public class GetReservationsPaginatedRequest : PaginatedQueryParameters {
    public string? Filter { get; set; }
}
