namespace Api.Controllers.Payload.Requests.Items;

public class AddItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Img { get; set; }
    public Guid CategoryId { get; set; }
    public List<Guid>? SizeIds { get; set; }
    public Dictionary<Guid, int> BranchPrices { get; set; }
}