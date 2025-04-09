namespace Domain.Entities;

public class ToppingPriceAtBranch: BaseEntity
{
    [ForeignKey("ToppingId")]
    public Guid ToppingId { get; set; }
    [ForeignKey("BranchId")]
    public Guid BranchId { get; set; }
    public double ToppingPrice { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual Topping Topping { get; set; }
    public virtual Branch Branch { get; set; }
}