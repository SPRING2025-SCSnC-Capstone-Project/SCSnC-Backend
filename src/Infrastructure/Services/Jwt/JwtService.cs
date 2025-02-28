using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Infrastructure.Services.Jwt;

public class JwtService(
        IOptions<JwtSettings> jwtOptions, 
        ECDsa signingKey, 
        IApplicationDbContext context
) : IJwtSService {
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    private readonly ECDsa _signingKey = signingKey;
    private readonly IApplicationDbContext _context = context;

    public Task<(string, string)> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public async Task<(JwtSecurityToken, RefreshToken)> SignInAsync(UserDto user, ClaimsIdentity claims, CancellationToken cancellationToken) {
        var (jwtToken, refreshToken) = await GenerateTokens(user, claims, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return (jwtToken, refreshToken);
    }

    private async Task<(JwtSecurityToken, RefreshToken)> GenerateTokens(UserDto user, ClaimsIdentity claims, CancellationToken cancellationToken) {
        var jwtToken = GenerateJwtToken(claims);
        var refreshToken = await GenerateUniqueRefreshToken(user);

        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await RemoveOldRefreshTokens(user, cancellationToken);

        return (jwtToken, refreshToken);
    } 

    private JwtSecurityToken GenerateJwtToken(ClaimsIdentity claims) {
        var publicSigningKey = new ECDsaSecurityKey(_signingKey) { KeyId = _jwtSettings.Secret };
        var tokenDescriptor = new SecurityTokenDescriptor() {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = claims,
            SigningCredentials = 
                new SigningCredentials(publicSigningKey, SecurityAlgorithms.EcdsaSha256),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return (JwtSecurityToken)token;
    }

    private async Task RemoveOldRefreshTokens(UserDto user, CancellationToken cancellationToken)
    {
        var existingRefreshTokens = (await _context.RefreshTokens.ToListAsync(cancellationToken))
            .Where(e => e.UserId == user.Id);
        var tokensToBeRemoved = existingRefreshTokens
            .Where(e => !e.IsActive
                        && e.CreationDateTime.PlusDays(_jwtSettings.RefreshTokenPersistenceTTLInDays) 
                            <= LocalDateTime.FromDateTime(DateTime.UtcNow));

        _context.RefreshTokens.RemoveRange([.. tokensToBeRemoved]);
    }


    private async Task<RefreshToken> GenerateUniqueRefreshToken(UserDto user) {
        var refreshToken = new RefreshToken() {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            CreationDateTime = LocalDateTime.FromDateTime(DateTime.UtcNow),
            ExpiryDateTime = LocalDateTime
                .FromDateTime(DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)),
        };

        var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken.Token);
        if (existingToken is not null) {
            return await GenerateUniqueRefreshToken(user);
        }

        return refreshToken;
    }
}
