using LibraryRestAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryRestAPI.Infrastructure.Contexts;

public sealed class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {
        
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<BookInSubscription> BookInSubscriptions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}