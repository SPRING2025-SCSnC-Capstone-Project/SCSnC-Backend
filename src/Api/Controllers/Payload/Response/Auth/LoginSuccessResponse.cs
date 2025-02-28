using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Response;

public class LoginSuccessResponse {
    public UserDto User { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null;
}
