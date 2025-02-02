using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public Role()
    {
        Users = new HashSet<User>();
    }
    public string RoleName { get; set; }
    
    public virtual ICollection<User> Users { get; set; }
}