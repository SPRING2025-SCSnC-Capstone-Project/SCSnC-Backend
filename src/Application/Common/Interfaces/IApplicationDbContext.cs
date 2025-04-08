using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Blog> Blogs { get; }
    public DbSet<BlogMedia> BlogMedias { get; }
    public DbSet<Branch> Branches { get; }
    public DbSet<Event> Events { get; }
    public DbSet<Feedback> Feedbacks { get; }
    public DbSet<IncludeTopping> IncludeToppings { get; }
    public DbSet<Item> Items { get; }
    public DbSet<ItemCategory> ItemCategories { get; }
    public DbSet<ItemPriceAtBranch> ItemPricesAtBranches { get; }
    public DbSet<ItemWithSize> ItemWithSizes { get; }
    public DbSet<JoinEvent> JoinEvents { get; }
    public DbSet<Order> Orders { get; }
    public DbSet<OrderDetail> OrderDetails { get; }
    public DbSet<Reservation> Reservations { get; }
    public DbSet<Size> Sizes { get; }
    public DbSet<Slot> Slots { get; }
    public DbSet<ReservedSlot> ReservedSlots { get; }
    public DbSet<EventSlot> EventSlots { get; }
    public DbSet<Table> Tables { get; }
    public DbSet<Topping> Toppings { get; }
    public DbSet<Transaction> Transactions { get; }
    public DbSet<User> Users { get; }
    public DbSet<UserVoucher> UserVouchers { get; }
    public DbSet<Voucher> Vouchers { get; }
    public DbSet<Workspace> Workspaces { get; }
    public DbSet<WorkspaceMedia> WorkspaceMedias { get; }
    public DbSet<WorkspaceType> WorkspaceTypes { get; }
    public DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
