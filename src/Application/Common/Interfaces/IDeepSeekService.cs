using Application.Common.Models.Dtos;

namespace Application.Common.Interfaces;

public interface IDeepSeekService
{
    public Task<ChatbotResponse> SendMessage(ChatbotRequest message);
}