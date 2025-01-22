using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    #region DbSet Properties
    
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemCategory> ItemCategories { get; set; }
    public virtual DbSet<JoinEvent> JoinEvents { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Size> Sizes { get; set; }
    public virtual DbSet<Slot> Slots { get; set; }
    public virtual DbSet<Table> Tables { get; set; }
    public virtual DbSet<Topping> Toppings { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserVoucher> UserVouchers { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<Workspace> Workspaces { get; set; }
    public virtual DbSet<WorkspaceType> WorkspaceTypes { get; set; }
    
    #endregion
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}