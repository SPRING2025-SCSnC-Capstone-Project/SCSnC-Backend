namespace Api.Controllers.Payload.Requests.Sizes;

public class AddSizeRequest
{
    public string SizeName { get; set; }
    public double PriceAdjustment { get; set; }
}