namespace Api.Controllers.Payload.Requests.Transactions;

public class TransactionDateRequest
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}