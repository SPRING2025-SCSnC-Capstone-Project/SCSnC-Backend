using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UsersController : ApiControllerBase {
    [HttpPost]
    public async Task<ActionResult<Result<UserDto>>> AddUser([FromBody] AddUserRequest request) {
        var command = new AddUserCommand(){
            Username = request.Username,
            Password = request.Password,
            Role = request.Role,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            FullName = request.FullName,
            AvatarLink = request.AvatarLink,
            BranchId = request.BranchId
        };

        var result = await Mediator.Send(command);
        return Ok(Result<UserDto>.Succeed(result));
    }

    [HttpGet("{userid:guid}")]
    public async Task<ActionResult<Result<UserDto>>> GetUserById([FromRoute] Guid userid) {
        var command = new GetUserByIdQuery(){
            Id = userid
        };

        var result = await Mediator.Send(command);
        return Ok(Result<UserDto>.Succeed(result));
    }

    [HttpGet]
    public async Task<ActionResult<Result<PaginatedList<UserDto>>>> GetUsers([FromQuery] GetUsersPaginatedRequest request) {
        var command = new GetUsersPaginatedQuery() {
            Page = request.Page,
            Size = request.Size,
            SortBy = request.SortBy,
            SortOrder = request.SortOrder,
            SearchTerm = request.SearchTerm
        };

        var result = await Mediator.Send(command);
        return Ok(Result<PaginatedList<UserDto>>.Succeed(result));
    }

    [HttpPut("{userid:guid}")]
    public async Task<ActionResult<Result<UserDto>>> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest request) {
        var command = new UpdateUserCommand() {
            Id = userId,
            AvatarLink = request.AvatarLink,
            Address = request.Address,
            Phone = request.Phone,
            FullName = request.FullName,
            Username = request.Username
        };

        var result = await Mediator.Send(command);
        return Ok(Result<UserDto>.Succeed(result));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<Result<UserDto>>> DeleteUser([FromRoute] Guid userId) {
        var command = new DeleteUserCommand() {
           Id = userId 
        };

        var result = await Mediator.Send(command);
        return Ok(Result<UserDto>.Succeed(result));
    }
}
