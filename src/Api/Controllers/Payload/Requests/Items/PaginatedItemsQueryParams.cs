namespace Api.Controllers.Payload.Requests.Items;

public class PaginatedItemsQueryParams
{
    /// <summary>
    /// Page number
    /// </summary>
    public int? Page { get; set; }
    /// <summary>
    /// Size number
    /// </summary>
    public int? Size { get; set; }
    /// <summary>
    /// Sort criteria
    /// </summary>
    public string? SortBy { get; set; }
    /// <summary>
    /// Sort direction
    /// </summary>
    public string? SortOrder { get; set; }
    /// <summary>
    /// Sort by category
    /// </summary>
    public string? FilterByCategory { get; set; }
    public Guid BranchId { get; set; }
}