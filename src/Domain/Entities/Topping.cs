using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Topping : BaseEntity
{
    public Topping()
    {
        IncludeToppings = new HashSet<IncludeTopping>();
        ToppingPricesAtBranches = new HashSet<ToppingPriceAtBranch>();
    }
    
    public string ToppingName { get; set; }
    public string ToppingDescription { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    
    public virtual ICollection<IncludeTopping> IncludeToppings { get; set; }
    public virtual ICollection<ToppingPriceAtBranch> ToppingPricesAtBranches { get; set; }
}