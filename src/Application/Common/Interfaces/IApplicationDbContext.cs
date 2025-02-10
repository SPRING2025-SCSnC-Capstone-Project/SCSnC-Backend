using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Event> Events { get; }
    public DbSet<Feedback> Feedbacks { get; }
    public DbSet<Item> Items { get; }
    public DbSet<ItemCategory> ItemCategories { get; }
    public DbSet<JoinEvent> JoinEvents { get; }
    public DbSet<Order> Orders { get; }
    public DbSet<OrderDetail> OrderDetails { get; }
    public DbSet<Reservation> Reservations { get; }
    public DbSet<Size> Sizes { get; }
    public DbSet<Slot> Slots { get; }
    public DbSet<Table> Tables { get; }
    public DbSet<Topping> Toppings { get; }
    public DbSet<User> Users { get; }
    public DbSet<UserVoucher> UserVouchers { get; }
    public DbSet<Voucher> Vouchers { get; }
    public DbSet<Workspace> Workspaces { get; }
    public DbSet<WorkspaceType> WorkspaceTypes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}