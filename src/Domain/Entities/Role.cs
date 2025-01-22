using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Role
{
    public Role()
    {
        Users = new HashSet<User>();
    }
    
    [Key]
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    
    public virtual ICollection<User> Users { get; set; }
}