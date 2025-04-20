using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    #region DbSet Properties
    
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<BlogMedia> BlogMedias => Set<BlogMedia>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<IncludeTopping> IncludeToppings => Set<IncludeTopping>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
    public DbSet<ItemPriceAtBranch> ItemPricesAtBranches => Set<ItemPriceAtBranch>();
    public DbSet<ItemWithSize> ItemWithSizes => Set<ItemWithSize>();
    public DbSet<JoinEvent> JoinEvents => Set<JoinEvent>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Size> Sizes => Set<Size>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<ReservedSlot> ReservedSlots => Set<ReservedSlot>();
    public DbSet<EventSlot> EventSlots => Set<EventSlot>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Topping> Toppings => Set<Topping>();
    public DbSet<ToppingPriceAtBranch> ToppingPricesAtBranches { get; set; }
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserVoucher> UserVouchers => Set<UserVoucher>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceMedia> WorkspaceMedias => Set<WorkspaceMedia>();
    public DbSet<WorkspaceType> WorkspaceTypes => Set<WorkspaceType>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<WorkspaceTypeAtBranch> WorkspaceTypeAtBranches => Set<WorkspaceTypeAtBranch>();

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
