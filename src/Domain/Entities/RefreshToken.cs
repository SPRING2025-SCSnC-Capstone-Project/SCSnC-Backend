namespace Domain.Entities;

public class RefreshToken
{
    [Key]
    public string Token { get; set; } = null!;

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    [NotMapped]
    public bool IsExpired => LocalDateTime.FromDateTime(DateTime.UtcNow) >= ExpiryDateTime;
    public LocalDateTime CreationDateTime { get; set; }
    public LocalDateTime ExpiryDateTime { get; set; }

    public bool IsUsed { get; set; }

    [NotMapped]
    public bool IsRevoked => RevocationDateTime.HasValue;
    public LocalDateTime? RevocationDateTime { get; set; }
    public string? RevocationReason { get; set; }
    public string? ReplacedBy { get; set; }

    [NotMapped]
    public bool IsActive => !IsUsed && !IsRevoked && !IsExpired;
}

