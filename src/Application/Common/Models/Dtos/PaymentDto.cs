namespace Application.Common.Models.Dtos;

public class PaymentResponse
{
    public string EntityId { get; set; }
    public string EntityType { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentMessage { get; set; }
    public decimal? Amount { get; set; }
}

public class VNPayRequest
{
    public decimal? vnp_Amount { get; set; }
    public string? vnp_Command { get; set; }
    public string? vnp_CreateDate { get; set; }
    // public string? vnp_CurrCode { get; set; }
    public string? vnp_BankCode { get; set; }
    public string? vnp_IpAddr { get; set; }
    public string? vnp_Locale { get; set; }
    public string? vnp_OrderInfo { get; set; }
    public string? vnp_OrderType { get; set; }
    public string? vnp_ReturnUrl { get; set; }
    // public string? vnp_TmnCode { get; set; }
    public string? vnp_ExpireDate { get; set; }
    public string? vnp_TxnRef { get; set; }
    // public string? vnp_Version { get; set; }
    public string? vnp_SecureHash { get; set; }
}

public class VNPayResponse
{
    public string vnp_TmnCode { get; set; } = string.Empty;
    public string vnp_BankCode { get; set; } = string.Empty;
    public string vnp_BankTranNo { get; set; } = string.Empty;
    public string vnp_CardType { get; set; } = string.Empty;
    public string vnp_OrderInfo { get; set; } = string.Empty;
    public string vnp_TransactionNo { get; set; } = string.Empty;
    public string vnp_TransactionStatus { get; set; } = string.Empty;
    public string vnp_TxnRef { get; set; } = string.Empty;
    public string vnp_SecureHashType { get; set; } = string.Empty;
    public string vnp_SecureHash { get; set; } = string.Empty;
    public int? vnp_Amount { get; set; }
    public string? vnp_ResponseCode { get; set; }
    public string vnp_PayDate { get; set; } = string.Empty;
}
public class VNPayConfig
{
    public string PaymentUrl { get; set; }
    public string ReturnUrl { get; set; }
    public string TmnCode { get; set; }
    public string HashSecret { get; set; }
    public string Version { get; set; }
    public string CurrencyCode { get; set; }
    public string Locale { get; set; }
}