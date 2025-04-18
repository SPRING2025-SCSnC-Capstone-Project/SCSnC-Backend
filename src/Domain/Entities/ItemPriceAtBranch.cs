namespace Domain.Entities;

public class ItemPriceAtBranch: BaseEntity
{
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    [ForeignKey("ItemId")]
    public Guid ItemId { get; set; }
    public double Price { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Branch Branch { get; set; }
    public virtual Item Item { get; set; } 
}