using Api.Controllers.Payload.Requests.ShiftSelections;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.ShiftSelections.Commands.RegisterShift;
using Application.ShiftSelections.Commands.UpdateShift;
using Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class EmployeesController : ApiControllerBase
{
    [HttpPost("{id:guid}/shifts")]
    [RequiresRole("Employee")]
    public async Task<ActionResult<List<ShiftSelectionDto>>> RegisterShift([FromRoute] Guid id, [FromBody] RegisterShiftRequest request)
    {
        var command = new RegisterShiftCommand()
        {
            UserId = request.UserId,
            BranchId = request.BranchId,
            WeekStart = request.WeekStart,
            DatesWithShiftTypeIds = request.DatesWithShiftTypeIds
        };

        var result = await Mediator.Send(command);
        return Ok(Result<List<ShiftSelectionDto>>.Succeed(result));
    }

    [HttpPut("{id:guid}/shifts")]
    [RequiresRole("Employee")]
    public async Task<ActionResult<List<ShiftSelectionDto>>> UpdateShift([FromRoute] Guid id, [FromBody] UpdateShiftRequest request)
    {
        var command = new UpdateShiftCommand() {
            Id = request.Id,
            UserId = request.UserId,
            ShiftSelectionUpdatesWithId = request.ShiftSelectionUpdatesWithId
        };

        var result = await Mediator.Send(command);
        return Ok(Result<List<ShiftSelectionDto>>.Succeed(result));
    }
}

