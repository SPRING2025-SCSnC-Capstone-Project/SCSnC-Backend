using System.Net;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Application.Orders.Common;
using Infrastructure.Services.VNPay.Common;

namespace Infrastructure.Services.VNPay;

public class VNPayService: IPaymentService
{
    public SortedList<string, string> requestData = new SortedList<string, string>(new VNPayCompare());
    public SortedList<string, string> responseData = new SortedList<string, string>(new VNPayCompare());
    
    public Task<string> GetPaymentLink(string baseUrl, string secretKey, VNPayRequest request)
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
        
        baseUrl += "?" + queryString;
        String signData = queryString;
        if (signData.Length > 0)
        {

            signData= signData.Remove(data.Length - 1, 1);
        }
        string vnp_SecureHash = HashHelper.HmacSHA512(secretKey , signData);
        baseUrl += "vnp_SecureHash=" + vnp_SecureHash;
           
        return Task.FromResult(baseUrl);
    }
    
    public void MakeRequestData(VNPayRequest request)
    {
        if (request.vnp_Amount != null)
            requestData.Add("vnp_Amount", request.vnp_Amount.ToString() ?? string.Empty);
        if (request.vnp_Command != null)
            requestData.Add("vnp_Command", request.vnp_Command);
        if (request.vnp_CreateDate != null)
            requestData.Add("vnp_CreateDate", request.vnp_CreateDate);
        if (request.vnp_CurrCode != null)
            requestData.Add("vnp_CurrCode", request.vnp_CurrCode);
        if (request.vnp_BankCode != null)
            requestData.Add("vnp_BankCode", request.vnp_BankCode);
        if (request.vnp_IpAddr != null)
            requestData.Add("vnp_IpAddr", request.vnp_IpAddr);
        if (request.vnp_Locale != null)
            requestData.Add("vnp_Locale", request.vnp_Locale);
        if (request.vnp_OrderInfo != null)
            requestData.Add("vnp_OrderInfo", request.vnp_OrderInfo);
        if (request.vnp_OrderType != null)
            requestData.Add("vnp_OrderType", request.vnp_OrderType);
        if (request.vnp_ReturnUrl != null)
            requestData.Add("vnp_ReturnUrl", request.vnp_ReturnUrl);
        if (request.vnp_TmnCode != null)
            requestData.Add("vnp_TmnCode", request.vnp_TmnCode);
        if (request.vnp_ExpireDate != null)
            requestData.Add("vnp_ExpireDate", request.vnp_ExpireDate);
        if (request.vnp_TxnRef != null)
            requestData.Add("vnp_TxnRef", request.vnp_TxnRef);
        if (request.vnp_Version != null)
            requestData.Add("vnp_Version", request.vnp_Version);
    }

    public async Task<bool> IsValidSignature(string secretKey, VNPayResponse response)
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
        string checkSum = HashHelper.HmacSHA512(secretKey,
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
}