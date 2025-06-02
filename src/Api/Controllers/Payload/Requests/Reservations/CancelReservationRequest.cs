using NodaTime;

namespace Api.Controllers.Payload.Requests;

public class CancelReservationRequest {
    public Guid UserId { get; set; }

}
