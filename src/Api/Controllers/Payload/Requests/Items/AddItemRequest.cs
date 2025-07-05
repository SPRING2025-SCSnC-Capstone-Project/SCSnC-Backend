namespace Api.Controllers.Payload.Requests.Items;

public class AddItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Img { get; set; }
    public Guid CategoryId { get; set; }
    public List<Guid> SizeIds { get; set; }
}