namespace Infrastructure.Services.VNPay;

public class VNPayConfig
{
    public const string Section = "VNPay";
    
    public string PaymentUrl { get; set; }
    public string ReturnUrl { get; set; }
    public string TmnCode { get; set; }
    public string HashSecret { get; set; }
    public string Version { get; set; }
    public string CurrencyCode { get; set; }
    public string Locale { get; set; }
}