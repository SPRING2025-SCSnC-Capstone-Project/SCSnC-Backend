namespace Api.Controllers.Payload.Requests.Events;
public class GetNewEventsInGivenDaysRequest : PaginatedQueryParameters
{
    public string? Filter { get; set; }
    public int GivenDays { get; set; } = 2;
}
