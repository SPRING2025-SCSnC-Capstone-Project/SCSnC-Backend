using Application.Common.Models.Dtos;

namespace Application.Common.Interfaces;

public interface IPaymentService
{
    public Task<string> GetPaymentLink(VNPayRequest request);
    public Task<bool> IsValidSignature(VNPayResponse response);
}