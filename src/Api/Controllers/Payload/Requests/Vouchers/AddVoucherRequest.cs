namespace Api.Controllers.Payload.Requests.Vouchers;

public class AddVoucherRequest
{
    public string VoucherCode { get; set; }
    public int DiscountValue { get; set; }
    public string Description { get; set; }
    public DateTime ExpiredDate { get; set; }
}