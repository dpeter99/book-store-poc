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

    public AppDbContext(DbContextOptions<AppDbContext> options,
        ITenantService tenantService) : base(options)
    {
        CurrentTenantId = tenantService.CurrentTenantId;
    }
    
    
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity type configurations
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());

        // Apply tenant query filters (requires CurrentTenantId context)
        modelBuilder.Entity<Book>()
            .HasQueryFilter(a => a.TenantId == CurrentTenantId);

        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.TenantId == CurrentTenantId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
