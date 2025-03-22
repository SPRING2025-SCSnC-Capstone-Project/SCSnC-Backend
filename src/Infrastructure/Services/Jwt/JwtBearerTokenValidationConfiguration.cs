using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Jwt;

public sealed class JwtBearerTokenValidationConfiguration(IOptions<JwtSettings> jwtSettings, ECDsa signingKey)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly ECDsa _signingKey = signingKey;

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new ECDsaSecurityKey(ECDsa.Create(_signingKey.ExportParameters(false))) { KeyId = _jwtSettings.Secret },
        };
    }
}
