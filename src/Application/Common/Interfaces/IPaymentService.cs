using Application.Common.Models.Dtos;

namespace Application.Common.Interfaces;

public interface IPaymentService
{
    public Task<string> GetPaymentLink(string baseUrl, string secretKey, VNPayRequest request);
    public Task<bool> IsValidSignature(string secretKey, VNPayResponse response);
}