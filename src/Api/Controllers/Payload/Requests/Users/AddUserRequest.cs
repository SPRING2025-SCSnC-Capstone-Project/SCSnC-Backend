namespace Api.Controllers.Payload.Requests;

public class AddUserRequest {
    public string? Password { get; set; }
    public string Username { get; set; } = null!;
    public string? FullName { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = null!;
    public string? AvatarLink { get; set; } 
    public Guid? BranchId { get; set; }
}
