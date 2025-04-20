namespace Domain.Entities;

public class Branch: BaseEntity
{
    public Branch()
    {
        ItemPricesAtBranches = new HashSet<ItemPriceAtBranch>();
        ToppingPricesAtBranches = new HashSet<ToppingPriceAtBranch>();
        Workspaces = new HashSet<Workspace>();
        Tables = new HashSet<Table>();
    }
    
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string BranchPhone { get; set; }
    public string BranchEmail { get; set; }
    public string BranchDescription { get; set; }
    public string BranchImage { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<ItemPriceAtBranch> ItemPricesAtBranches { get; set; }
    public virtual ICollection<ToppingPriceAtBranch> ToppingPricesAtBranches { get; set; }
    public virtual ICollection<Workspace> Workspaces { get; set; }
    public virtual ICollection<Table> Tables { get; set; }
}