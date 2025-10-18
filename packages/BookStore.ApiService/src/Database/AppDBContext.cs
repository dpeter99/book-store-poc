using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;
using Tenant = BookStore.ApiService.Database.Entities.Tenant;

namespace BookStore.ApiService.Database;

public class AppDbContext : DbContext
{
    public Guid? CurrentTenantId { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options,
        ITenantService tenantService) : base(options)
    {
        CurrentTenantId = tenantService.CurrentTenantId;
    }
    
    
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Book> Books { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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