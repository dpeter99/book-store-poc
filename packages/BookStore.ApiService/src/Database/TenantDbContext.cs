using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Database;

public class TenantDbContext: DbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseSeeding(((context, b) =>
        {
            context.Set<Tenant>()
                .Add(new Tenant()
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Domain = "test",
                });
            context.SaveChanges();
        }));
        optionsBuilder.UseAsyncSeeding((async (context, b, token) =>
        {
            context.Set<Tenant>()
                .Add(new Tenant()
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Domain = "test",
                });
            await context.SaveChangesAsync(token);
        }));
    }
}