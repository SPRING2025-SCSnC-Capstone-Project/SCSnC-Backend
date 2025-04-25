namespace Api.Controllers.Payload.Requests;

public class LoginRequest {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid? Branch {  get; set; }
}
