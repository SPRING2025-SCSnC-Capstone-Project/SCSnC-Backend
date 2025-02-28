namespace Infrastructure.Services.Jwt;

public class JwtSettings {
    public const string Section = "JwtSettings";

    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public int TokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
    public int RefreshTokenPersistenceTTLInDays { get; set; }
}
