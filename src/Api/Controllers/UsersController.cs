using Api.Controllers.Payload.Requests;
using Application.Common.Models;
using Application.Common.Models.Dtos;
using Application.Users.Commands;
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
        };

        var result = await Mediator.Send(command);
        return Ok(Result<UserDto>.Succeed(result));
    }
}
