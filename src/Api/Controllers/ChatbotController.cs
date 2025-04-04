using Application.Chatbot.Commands;
using Application.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ChatbotController: ApiControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatbotRequest request)
    {
        var command = new SendMessageCommand()
        {
            Request = request.Request.ToString()
        };
        
        var response = await Mediator.Send(command);
        
        return Ok(response);
    }
}