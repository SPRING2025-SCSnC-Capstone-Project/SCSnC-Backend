namespace Api.Controllers.Payload.Requests.Vouchers;

public class UpdateVoucherRequest
{
    public string VoucherCode { get; set; }
    public int DiscountValue { get; set; }
    public string Description { get; set; }
    public DateTime ExpiredDate { get; set; }
    public bool IsActive { get; set; }
}