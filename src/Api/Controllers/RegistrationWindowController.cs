using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.RegistrationWindows.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class RegistrationWindowController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<RegistrationWindowDto>>> CreateRegistrationWindow([FromBody] CreateRegistrationWindowRequest request)
    {
        var command = new CreateRegistrationWindowCommand()
        {
            WeekStart = request.WeekStart,
            OpenAt = request.OpenAt,
            CloseAt = request.CloseAt,
            BranchId = request.BranchId
        };
        var result = await Mediator.Send(command);
        return Ok(Result<RegistrationWindowDto>.Succeed(result));
    }
}
