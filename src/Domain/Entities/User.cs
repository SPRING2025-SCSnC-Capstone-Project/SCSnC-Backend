using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    public User()
    {
        Orders = new HashSet<Order>();
        UserVouchers = new HashSet<UserVoucher>();
        JoinEvents = new HashSet<JoinEvent>();
        Reservations = new HashSet<Reservation>();
    }
    
    [Key]
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    [ForeignKey("RoleId")]
    public Guid RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    public virtual Role Role { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<UserVoucher> UserVouchers { get; set; }
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
}