using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Database;

public class AppDbContext(
    DbContextOptions options,
    ICurrentTenantService tenantService
    ) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
             .HasQueryFilter(a => a.Id == tenantService.TenantId);
    }
}