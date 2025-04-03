using Application.Chatbot.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ChatbotController: ApiControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody]string request)
    {
        var command = new SendMessageCommand()
        {
            Request = request
        };
        
        var response = await Mediator.Send(command);
        
        return Ok(response);
    }
}