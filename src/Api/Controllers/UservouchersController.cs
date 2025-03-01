using Api.Controllers.Payload.Requests;
using Api.Controllers.Payload.Requests.UserVouchers;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.UserVouchers.Commands.CreateUserVoucher;
using Application.UserVouchers.Queries.GetUserVoucherById;
using Application.UserVouchers.Queries.GetUserVouchersPaginated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UservouchersController: ApiControllerBase
{
    // The following functions are advised to be put in UsersControllers.cs
    [HttpGet("{user_id:guid}")]
    public async Task<ActionResult<Result<PaginatedList<UserVoucherDto>>>> GetUserVouchersByUserId([FromRoute] Guid user_id, [FromQuery] PaginatedQueryParameters request)
    {
        var query = new GetUserVouchersPaginatedQuery()
        {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            UserId = user_id
        };
    
        var result = await Mediator.Send(query);
    
        return Ok(Result<PaginatedList<UserVoucherDto>>.Succeed(result));
    }
    
    [HttpGet("{uservoucher_id:guid}")]
    public async Task<ActionResult<Result<UserVoucherDto>>> GetUserVoucherById([FromRoute] Guid uservoucher_id)
    {
        var query = new GetUserVoucherByIdQuery()
        {
            Id = uservoucher_id
        };
    
        var result = await Mediator.Send(query);
    
        return Ok(Result<UserVoucherDto>.Succeed(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<Result<UserVoucherDto>>> AddUserVoucher([FromBody] AddUserVoucherRequest request)
    {
        var command = new CreateUserVoucherCommand()
        {
            UserId = request.UserId,
            VoucherId = request.VoucherId
        };
    
        var result = await Mediator.Send(command);
    
        return Ok(Result<UserVoucherDto>.Succeed(result));
    }
}