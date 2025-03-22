using System.Globalization;
using System.Net;
using System.Text;
using System.Web;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Infrastructure.Services.VNPay.Common;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.VNPay;

public class VNPayService(IOptions<VNPayConfig> vnPayConfig) : IPaymentService
{
    private readonly VNPayConfig _config = vnPayConfig.Value;

    public SortedList<string, string> requestData = new SortedList<string, string>(new VNPayCompare());
    public SortedList<string, string> responseData = new SortedList<string, string>(new VNPayCompare());

    public Task<string> GetPaymentLink(VNPayRequest request)
    {
        MakeRequestData(request);
        StringBuilder data = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in requestData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }

        string queryString = data.ToString();

        string paymentUrl = _config.PaymentUrl + "?" + queryString;
        String signData = queryString;
        if (signData.Length > 0)
        {
            signData = signData.Remove(data.Length - 1, 1);
        }

        string vnp_SecureHash = HashHelper.HmacSHA512(_config.HashSecret, signData);
        paymentUrl += "vnp_SecureHash=" + vnp_SecureHash;

        return Task.FromResult(paymentUrl);
    }

    public void MakeRequestData(VNPayRequest request)
    {
        if (request.vnp_Amount != null)
            requestData.Add("vnp_Amount", request.vnp_Amount.ToString() ?? string.Empty);
        if (request.vnp_Command != null)
            requestData.Add("vnp_Command", request.vnp_Command);
        if (request.vnp_CreateDate != null)
            requestData.Add("vnp_CreateDate", request.vnp_CreateDate);
        requestData.Add("vnp_CurrCode", _config.CurrencyCode);
        if (request.vnp_BankCode != null)
            requestData.Add("vnp_BankCode", request.vnp_BankCode);
        if (request.vnp_IpAddr != null)
            requestData.Add("vnp_IpAddr", request.vnp_IpAddr);
        requestData.Add("vnp_Locale", _config.Locale);
        if (request.vnp_OrderInfo != null)
            requestData.Add("vnp_OrderInfo", HttpUtility.UrlEncode(request.vnp_OrderInfo));
        if (request.vnp_OrderType != null)
            requestData.Add("vnp_OrderType", request.vnp_OrderType);
        requestData.Add("vnp_ReturnUrl", _config.ReturnUrl);
        requestData.Add("vnp_TmnCode", _config.TmnCode);
        if (request.vnp_TxnRef != null)
            requestData.Add("vnp_TxnRef", request.vnp_TxnRef);
        if (request.vnp_SecureHash != null) 
            requestData.Add("vnp_SecureHash", request.vnp_SecureHash);
        requestData.Add("vnp_Version", _config.Version);
    }

    public async Task<bool> IsValidSignature(VNPayResponse response)
    {
        MakeResponseData(response);
        StringBuilder data = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in responseData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }

        string checkSum = HashHelper.HmacSHA512(_config.HashSecret,
            data.ToString().Remove(data.Length - 1, 1));
        return checkSum.Equals(response.vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
    }

    public void MakeResponseData(VNPayResponse response)
    {
        if (response.vnp_Amount != null)
            responseData.Add("vnp_Amount", response.vnp_Amount.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_TmnCode))
            responseData.Add("vnp_TmnCode", response.vnp_TmnCode.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_BankCode))
            responseData.Add("vnp_BankCode", response.vnp_BankCode.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_BankTranNo))
            responseData.Add("vnp_BankTranNo", response.vnp_BankTranNo.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_CardType))
            responseData.Add("vnp_CardType", response.vnp_CardType.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_OrderInfo))
            responseData.Add("vnp_OrderInfo", response.vnp_OrderInfo.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_TransactionNo))
            responseData.Add("vnp_TransactionNo", response.vnp_TransactionNo.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_TransactionStatus))
            responseData.Add("vnp_TransactionStatus", response.vnp_TransactionStatus.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_TxnRef))
            responseData.Add("vnp_TxnRef", response.vnp_TxnRef.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_PayDate))
            responseData.Add("vnp_PayDate", response.vnp_PayDate.ToString() ?? string.Empty);
        if (!string.IsNullOrEmpty(response.vnp_ResponseCode))
            responseData.Add("vnp_ResponseCode", response.vnp_ResponseCode ?? string.Empty);
    }
    
    public class VNPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}