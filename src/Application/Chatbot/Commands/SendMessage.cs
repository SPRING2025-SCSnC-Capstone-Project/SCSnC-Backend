using Application.Common.Interfaces;
using Application.Common.Models.Dtos;

namespace Application.Chatbot.Commands;

public record SendMessageCommand : IRequest<ChatbotResponse>
{
    public string Request { get; set; }
}

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatbotResponse>
{
    private readonly IDeepSeekService _chatbotService;
    
    public SendMessageCommandHandler(IDeepSeekService chatbotService)
    {
        _chatbotService = chatbotService;
    }
    
    public async Task<ChatbotResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var response = await _chatbotService.SendMessage(new ChatbotRequest()
        {
            Request = request.Request
        });

        return response;
    }
}