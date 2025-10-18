using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;
using Tenant = BookStore.ApiService.Database.Entities.Tenant;

namespace BookStore.ApiService.Database;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ITenantService tenantService
    ) : DbContext(options)
{
    public Guid? CurrentTenantId { get; set; } = tenantService.CurrentTenantId;
    
    
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Book> Books { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
             .HasQueryFilter(a => a.Id == CurrentTenantId);
        
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.Id == CurrentTenantId);
            
    }
}