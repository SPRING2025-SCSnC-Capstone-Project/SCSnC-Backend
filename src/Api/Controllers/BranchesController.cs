using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.Branches;
using Api.Controllers.Payload.Requests.RegistrationWindow;
using Api.Controllers.Payload.Requests.ShiftSelections;
using Application.Branches.Commands.CreateBranch;
using Application.Branches.Commands.DeleteBranch;
using Application.Branches.Commands.UpdateBranch;
using Application.Branches.Queries.GetBranchById;
using Application.Branches.Queries.GetBranchesPaginated;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.RegistrationWindows.Commands.UpdateRegistrationWindow;
using Application.ShiftSelections.Queries.GetShiftsByEmployeeAndWeekStart;
using Application.ShiftSelections.Queries.GetShiftsByWeekStart;
using Application.ShiftSelections.Queries.GetTotalWorkHoursOfWeek;
using Application.ShiftTypes.Queries;
using Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BranchesController : ApiControllerBase
{
    #region Basic CRUD Operations
    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<BranchDto>>>> GetBranches([FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetBranchesPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        };

        var result = await Mediator.Send(query);

        return Ok(Result<PaginatedList<BranchDto>>.Succeed(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> GetBranchById([FromRoute] Guid id)
    {
        var query = new GetBranchByIdQuery()
        {
            Id = id
        };

        var result = await Mediator.Send(query);

        return Ok(Result<BranchDto>.Succeed(result));
    }

    [HttpPost]
    public async Task<ActionResult<Result<BranchDto>>> AddBranch([FromBody] AddBranchRequest request)
    {
        var command = new CreateBranchCommand()
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.Phone,
            Email = request.Email,
            ImgUrl = request.ImgUrl,
            Description = request.Description
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> UpdateBranch([FromRoute] Guid id, [FromBody] UpdateBranchRequest request)
    {
        var command = new UpdateBranchCommand()
        {
            Id = id,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            ImgUrl = request.ImgUrl,
            Description = request.Description
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result<BranchDto>>> DeleteBranch([FromRoute] Guid id)
    {
        var command = new DeleteBranchCommand()
        {
            Id = id
        };

        var result = await Mediator.Send(command);

        return Ok(Result<BranchDto>.Succeed(result));
    }
    #endregion

    [Authorize]
    [RequiresRole("Manager")]
    [HttpGet("{id:guid}/shifts")]
    public async Task<ActionResult<Result<PaginatedList<ShiftTypeDto>>>> GetBranchShiftsPaginated([FromRoute] Guid branchId, [FromQuery] PaginatedQueryParameters request)
    {
        var command = new GetShiftTypesByBranchQuery()
        {
            BranchId = branchId,
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder
        };

        var result = await Mediator.Send(command);

        return Ok(Result<PaginatedList<ShiftTypeDto>>.Succeed(result));
    }

    [Authorize]
    [RequiresRole("Manager")]
    [HttpPut("{id:guid}/registration-window")]
    public async Task<ActionResult<Result<RegistrationWindowDto>>> UpdateRegistrationWindow([FromRoute] Guid id, [FromBody] UpdateRegistrationWindowRequest request)
    {
        var command = new UpdateRegistrationWindowCommand()
        {
            BranchId = id,
            WeekStart = request.WeekStart,
            OpenAt = request.OpenAt,
            CloseAt = request.CloseAt
        };

        var result = await Mediator.Send(command);

        return Ok(Result<RegistrationWindowDto>.Succeed(result));
    }

    [Authorize]
    [RequiresRole("Manager")]
    [HttpGet("{branchId:guid}/schedules/summary")]
    public async Task<ActionResult<Result<List<ShiftSummaryDto>>>> GetShiftSchedulesSummary([FromRoute] Guid branchId, [FromQuery] GetShiftSummaryRequest request)
    {
        var command = new GetShiftsByWeekStartQuery() {
            BranchId = branchId,
            WeekStart = request.WeekStart
        };

        var result = await Mediator.Send(command);

        return Ok(Result<List<ShiftSummaryDto>>.Succeed(result));
    }

    [Authorize]
    [RequiresRole("Manager")]
    [HttpGet("{branchId:guid}/employees/{employeeId:guid}/schedule")]
    public async Task<ActionResult<Result<ScheduleDto>>> GetEmployeeSchedule([FromRoute] Guid branchId, [FromRoute] Guid employeeId, [FromQuery] DateOnly weekStart) {
        var command = new GetShiftsByEmployeeAndWeekStartQuery() {
            BranchId = branchId,
            EmployeeId = employeeId,
            WeekStart = weekStart
        };

        var result = await Mediator.Send(command);

        return Ok(Result<ScheduleDto>.Succeed(result));
    }

    [Authorize]
    [RequiresRole("Manager")]
    [HttpGet("{branchId:guid}/employees/{employeeId:guid}/hours")]
    public async Task<ActionResult<Result<WorkHoursDto>>> GetEmployeeTotalWorkHours([FromRoute] Guid branchId, [FromRoute] Guid employeeId, [FromQuery] DateOnly weekStart) {
        var command = new GetTotalWorkHoursOfWeekQuery() {
            BranchId = branchId,
            EmployeeId = employeeId,
            WeekStart = weekStart
        };

        var result = await Mediator.Send(command);

        return Ok(Result<WorkHoursDto>.Succeed(result));
    }
}