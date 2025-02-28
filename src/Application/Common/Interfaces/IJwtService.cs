using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Models.Dtos;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtSService {
    public Task<(JwtSecurityToken, RefreshToken)> SignInAsync(UserDto user, ClaimsIdentity claims, CancellationToken cancellationToken);
    public Task<(string, string)> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
