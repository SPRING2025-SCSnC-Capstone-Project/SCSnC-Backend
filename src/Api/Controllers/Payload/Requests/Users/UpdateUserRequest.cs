namespace Api.Controllers.Payload.Requests;

public class UpdateUserRequest {
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? AvatarLink { get; set; }
} 
