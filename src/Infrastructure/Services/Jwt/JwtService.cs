using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Common.Interfaces;
using Application.Common.Models.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Infrastructure.Services.Jwt;

public class JwtService(
        IOptions<JwtSettings> jwtOptions, 
        ECDsa signingKey, 
        IApplicationDbContext context,
        IMapper mapper
) : IJwtSService {
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    private readonly ECDsa _signingKey = signingKey;
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<(JwtSecurityToken, RefreshToken)> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken) {
        var (storedRefreshToken, user) = await ValidateRefreshToken(refreshToken, cancellationToken);
        if (storedRefreshToken.IsRevoked)
        {
            await RevokeDescendentRefreshToken(storedRefreshToken, cancellationToken,
                reason: $"Attempted use of revoked ancestor token: {refreshToken}");
            throw new SecurityTokenValidationException("Token has already been revoked.");
        }
        if (storedRefreshToken.IsUsed)
        {
            await RevokeDescendentRefreshToken(storedRefreshToken, cancellationToken,
                reason: $"Attempted reuse of ancestor token: {refreshToken}");
            throw new SecurityTokenValidationException("Token has already been used.");
        }
        if (storedRefreshToken.IsExpired)
        {
            throw new SecurityTokenValidationException("Token has expired.");
        }

        var (token, newRefreshToken) = GenerateTokens(user, cancellationToken).Result;

        // Rotate the refresh token
        storedRefreshToken.RevocationDateTime = LocalDateTime.FromDateTime(DateTime.UtcNow);
        storedRefreshToken.ReplacedBy = newRefreshToken.Token;
        storedRefreshToken.IsUsed = true;
        storedRefreshToken.RevocationReason = "Rotated upon token renewal";
        _context.RefreshTokens.Update(storedRefreshToken);

        await _context.SaveChangesAsync(cancellationToken);
        return (token, newRefreshToken);        
    }

    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }

    public async Task<(JwtSecurityToken, RefreshToken)> SignInAsync(UserDto user, CancellationToken cancellationToken) {
        var (jwtToken, refreshToken) = await GenerateTokens(user, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return (jwtToken, refreshToken);
    }

    private async Task<(JwtSecurityToken, RefreshToken)> GenerateTokens(UserDto user, CancellationToken cancellationToken) {
        var claims = new ClaimsIdentity();

        claims.AddClaims([
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
        ]);

        if (user.FullName != null) {
            claims.AddClaim(new(JwtRegisteredClaimNames.Name, user.FullName));
        }

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

    private async Task<(RefreshToken, UserDto)> ValidateRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var storedRefreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(e => e.Token == refreshToken, cancellationToken)
            ?? throw new SecurityTokenValidationException("Invalid token.");

        var user = await _context.Users
            .SingleOrDefaultAsync(e => e.Id == storedRefreshToken.UserId, cancellationToken)
            ?? throw new SecurityTokenValidationException("User associated with token does not exist.");

        return (storedRefreshToken, _mapper.Map<UserDto>(user));
    }

    public async Task LogoutAsync(string token, string refreshToken)
    {
        ClaimsPrincipal? validatedToken;
        var handler = new JwtSecurityTokenHandler();
        try
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new ECDsaSecurityKey(_signingKey) { KeyId = _jwtSettings.Secret },
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
            };
            validatedToken = handler.ValidateToken(token, tokenValidationParameters, out _);

        }
        catch
        {
            validatedToken = null;
        }

        if (validatedToken is null)
        {
            throw new AuthenticationFailureException("Invalid token.");
        }

        var jti = validatedToken.Claims.Single(x => x.Type.Equals(JwtRegisteredClaimNames.NameId)).Value;
        var storedRefreshToken =
            await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token.Equals(refreshToken));

        if (storedRefreshToken is null) return;
        
        if (!storedRefreshToken.UserId.Equals(jti))
        {
            throw new AuthenticationFailureException("This refresh token does not match this Jwt.");
        }

        _context.RefreshTokens.Remove(storedRefreshToken);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
    
    private async Task RevokeDescendentRefreshToken(
        RefreshToken refreshToken, CancellationToken cancellationToken,
        string? reason = null)
    {
        // Recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedBy))
        {
            var childToken = await _context.RefreshTokens
                                       .SingleOrDefaultAsync(e => e.Token == refreshToken.ReplacedBy, cancellationToken);

            // A descendent token is always newer than the one it replaced, there can never be a situation where
            // a token is preserved but its replacement has been removed.
            if (!childToken!.IsActive)
            {
                await RevokeDescendentRefreshToken(childToken, cancellationToken, reason);
            }
            else
            {
                RevokeRefreshTokenInternal(childToken, cancellationToken, reason);
            }
        }
    }
    
    private void RevokeRefreshTokenInternal(
        RefreshToken refreshToken, CancellationToken cancellationToken,
        string? reason = null, RefreshToken? replacementToken = null)
    {
        refreshToken.RevocationDateTime = LocalDateTime.FromDateTime(DateTime.UtcNow);
        refreshToken.RevocationReason = reason;
        refreshToken.ReplacedBy = replacementToken?.Token;

        _context.RefreshTokens.Update(refreshToken);
    }
}
