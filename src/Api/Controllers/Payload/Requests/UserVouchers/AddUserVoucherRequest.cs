namespace Api.Controllers.Payload.Requests.UserVouchers;

public class AddUserVoucherRequest
{
    public Guid UserId { get; set; }
    public Guid VoucherId { get; set; }
}