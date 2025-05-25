using Api.Controllers.Payload.Requests.ShiftTypes;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.ShiftTypes.Commands.CreateShiftType;
using Application.ShiftTypes.Commands.DeleteShiftType;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ShiftTypesController: ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<ReturnShiftTypeDto>>> AddShiftType([FromBody] AddShiftTypeRequest request)
    {
        var command = new CreateShiftTypeCommand()
        {
            BranchId = request.BranchId,
            Name = request.Name,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ReturnShiftTypeDto>.Succeed(result));
    }
    
    [HttpDelete("{ShiftTypeId:guid}")]
    public async Task<ActionResult<Result<ReturnShiftTypeDto>>> DeleteShiftType([FromRoute] Guid ShiftTypeId)
    {
        var command = new DeleteShiftTypeCommand()
        {
            ShiftTypeId = ShiftTypeId,
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ReturnShiftTypeDto>.Succeed(result));
    }
    
    
}