namespace Api.Controllers.Payload.Requests.Items;

public class UpdateItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Img { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateItemPriceRequest
{
    public Guid BranchId { get; set; }
    public double Price { get; set; }
}