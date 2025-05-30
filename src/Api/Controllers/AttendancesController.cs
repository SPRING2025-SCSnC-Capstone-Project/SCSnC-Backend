using Api.Controllers.Payload.Requests.Attendances;
using Application.AttendanceRecords.Commands.CheckIn;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class AttendancesController : ApiControllerBase {
    [HttpPost("check-in")]
    [RequiresRole("Employee")]
    public async Task<ActionResult<Result<AttendanceRecordDto>>> CheckIn(CheckInRequest request) {
        var command = new CheckInCommand() {
            BranchId = request.BranchId,
            ShiftTypeId = request.ShiftTypeId,
            EmployeeId = request.EmployeeId,
        };

        var result = await Mediator.Send(command);
        return Ok(Result<AttendanceRecordDto>.Succeed(result));
    }
}
