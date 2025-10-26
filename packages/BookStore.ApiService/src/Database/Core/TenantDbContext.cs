using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Database;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
    public required DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TenantEntityTypeConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
