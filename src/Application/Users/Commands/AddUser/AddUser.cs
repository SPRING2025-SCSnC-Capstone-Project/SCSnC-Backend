using Application.Common.Models.Dtos;

namespace Application.Users.Commands;

public record AddUserCommand : IRequest<UserDto> {
    public string Username { get; init; }
    public string Password { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Address { get; init; }
    public string Role { get; init; }
    public string? AvatarLink { get; init; }
}

// public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto> {

// }