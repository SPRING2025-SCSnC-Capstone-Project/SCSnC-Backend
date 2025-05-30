using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtSService {
    public Task<(JwtSecurityToken, RefreshToken)> SignInAsync(UserDto user, CancellationToken cancellationToken);
    public Task<(JwtSecurityToken, RefreshToken)> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken);
    public Task LogoutAsync(string token, string refreshToken);
}
