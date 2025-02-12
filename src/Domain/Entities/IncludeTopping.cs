using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class IncludeTopping : BaseEntity
{
    [ForeignKey("ToppingId")]
    public Guid ToppingId { get; set; }
    [ForeignKey("OrderDetailId")]
    public Guid OrderDetailId { get; set; }
    
    public virtual Topping Topping { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
}