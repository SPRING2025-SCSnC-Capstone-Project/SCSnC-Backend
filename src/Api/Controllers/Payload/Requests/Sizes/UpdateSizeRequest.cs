namespace Api.Controllers.Payload.Requests.Sizes;

public class UpdateSizeRequest
{
    public string SizeName { get; set; }
    public double PriceAdjustment { get; set; }
}