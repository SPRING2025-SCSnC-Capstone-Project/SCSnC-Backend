using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        Orders = new HashSet<Order>();
        UserVouchers = new HashSet<UserVoucher>();
        JoinEvents = new HashSet<JoinEvent>();
        Reservations = new HashSet<Reservation>();
        Blogs = new HashSet<Blog>();
        Events = new HashSet<Event>();
    }
    public string AccountType { get; set; }
    public string Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; }
    public string? AvatarLink { get; set; }
    public bool IsActive { get; set; }
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime LastUpdatedAt { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<UserVoucher> UserVouchers { get; set; }
    public virtual ICollection<Event> Events { get; set; }
    public virtual ICollection<JoinEvent> JoinEvents { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual ICollection<Blog> Blogs { get; set; }
}
