namespace Api.Controllers.Payload.Requests.ItemCategories;

public class UpdateItemCategoryRequest
{
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
}