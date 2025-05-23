using NodaTime;

namespace Api.Controllers.Payload.Requests;

public class CancelReservationRequest {
    public Guid ReservationId { get; set; }
    public bool IsCanceled { get; set; }

}
