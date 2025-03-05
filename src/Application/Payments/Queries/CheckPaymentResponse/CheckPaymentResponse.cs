using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Payments.Queries.CheckPaymentResponse;

public record CheckPaymentResponseQuery : IRequest<PaymentResponse>
{
    public VNPayResponse vnpayResponse { get; init; }
}

public class CheckPaymentResponseQueryHandler : IRequestHandler<CheckPaymentResponseQuery, PaymentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public CheckPaymentResponseQueryHandler(IApplicationDbContext context, IMapper mapper,
        IPaymentService paymentService)
    {
        _context = context;
        _mapper = mapper;
        _paymentService = paymentService;
    }


    public async Task<PaymentResponse> Handle(CheckPaymentResponseQuery request, CancellationToken cancellationToken)
    {
        VNPayConfig vNPayConfig = VNPayHelper.GetConfigData();
        PaymentResponse paymentResponse = new PaymentResponse();

        paymentResponse.OrderId = request.vnpayResponse.vnp_TxnRef;
        paymentResponse.Amount = request.vnpayResponse.vnp_Amount;

        bool isValid = await _paymentService.IsValidSignature(vNPayConfig.HashSecret, request.vnpayResponse);
        if (isValid)
        {
            if (await _context.Orders.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.vnpayResponse.vnp_TxnRef)) !=
                null)
            {
                if (request.vnpayResponse.vnp_ResponseCode == "00")
                {
                    paymentResponse.PaymentStatus = "Success";
                }
                else
                {
                    paymentResponse.PaymentStatus = "Failed";
                }

                switch (request.vnpayResponse.vnp_ResponseCode)
                {
                    case "00":
                        paymentResponse.PaymentMessage = "Successful transaction.";
                        PaymentHelper.UpdateStatus(request.vnpayResponse.vnp_TxnRef, _context, cancellationToken);
                        break;
                    case "07":
                        paymentResponse.PaymentMessage =
                            "Successful balance deduction. Suspicious transaction (Related to scam, abnormal transaction).";
                        break;
                    case "09":
                        paymentResponse.PaymentMessage = "Card/Banking account is not registered banking services.";
                        break;
                    case "10":
                        paymentResponse.PaymentMessage =
                            "Incorrect Card/Banking Account infomation validation more than 3 times.";
                        break;
                    case "11":
                        paymentResponse.PaymentMessage =
                            "Transaction duration expired. Please redo making transaction.";
                        break;
                    case "12":
                        paymentResponse.PaymentMessage = "Card/Banking Account is currently unavailable (Locked).";
                        break;
                    case "13":
                        paymentResponse.PaymentMessage = "Wrong OTP Code inputed.";
                        break;
                    case "24":
                        paymentResponse.PaymentMessage = "Transaction Canceled.";
                        break;
                    case "51":
                        paymentResponse.PaymentMessage =
                            "Banking Account's balance is not enough for this transaction.";
                        break;
                    case "65":
                        paymentResponse.PaymentMessage = "Bankiing Account exceeds the transaction limitation per day.";
                        break;
                    case "75":
                        paymentResponse.PaymentMessage = "Bank in maintanance.";
                        break;
                    case "79":
                        paymentResponse.PaymentMessage =
                            "Incorrect transaction's password inputed more than specified number of times.";
                        break;
                    case "99":
                        paymentResponse.PaymentMessage = "Other Transaction Error.";
                        break;
                }
            }
            else
            {
                paymentResponse.PaymentStatus = "Failed";
                paymentResponse.PaymentMessage = "Can't find order in DB.";
            }
        }
        else
        {
            paymentResponse.PaymentStatus = "Failed";
            paymentResponse.PaymentMessage = "Invalid signature in response.";
        }
        
        

        return paymentResponse;
    }
}