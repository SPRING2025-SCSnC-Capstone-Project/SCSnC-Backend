namespace Api.Controllers.Payload.Requests.Branches;

public class UpdateBranchRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string ImgUrl { get; set; }
}