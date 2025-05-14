namespace Api.Controllers.Payload.Requests.UtilityServices;

public class UpdateUtilityServiceRequest
{
    public string ServiceName { get; set; }
    public string ServiceImage { get; set; }
    public double ServiceFee { get; set; }
}