using Application.Common.Models.Dtos;

namespace Api.Controllers.Payload.Response;

public class RefreshTokenResponse {
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null;
}
