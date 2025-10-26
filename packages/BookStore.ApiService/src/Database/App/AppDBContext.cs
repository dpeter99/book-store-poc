using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.Database.Entities.Modules.Users;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;
using Tenant = BookStore.ApiService.Database.Entities.Tenant;

namespace BookStore.ApiService.Database;

public class AppDbContext : DbContext
{
    public TenantId? CurrentTenantId { get; set; }
    public string? CurrentTenantConnectionString { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options,
        ICurrentTenantService tenantService) : base(options)
    {
        CurrentTenantId = tenantService.CurrentTenantId;
        CurrentTenantConnectionString = tenantService.CurrentTenantConnectionString;
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity type configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Apply tenant query filters (requires CurrentTenantId context)
        modelBuilder.Entity<Book>()
            .HasQueryFilter(a => a.TenantId == CurrentTenantId);

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.TenantId == CurrentTenantId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (CurrentTenantConnectionString is not null)
        {
            optionsBuilder.UseNpgsql(CurrentTenantConnectionString);
        }
    }
}
