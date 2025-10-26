using System.Diagnostics;
using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Infrastructure.MuliTenant;

public record CreateTenantData(string name, string domain);

public interface ITenantService
{
    Task<Tenant> Create(CreateTenantData data);
}

public class TenantService(TenantDbContext db, IConfiguration configuration, IServiceProvider serviceProvider) : ITenantService
{
    private readonly ActivitySource _activitySource = new ActivitySource(nameof(TenantService));
    
    public async Task<Tenant> Create(CreateTenantData data)
    {
        string connectionString = await CreateNewDb(data.name);

        Tenant tenant = new()
        {
            Name = data.name,
            Domain = data.domain,
            ConnectionString = connectionString
        };
        db.Tenants.Add(tenant);
        await db.SaveChangesAsync();

        return tenant;
    }

    private async Task<string> CreateNewDb(string slug)
    {
        using var activity = _activitySource.StartActivity();
        
        string dbName = $"bookstore-{slug}";
        string baseConnectionString = configuration.GetConnectionString("bookdb")!;
        string newConnectionString = baseConnectionString.Replace("bookdb", dbName);

        try
        {
            using var scope = serviceProvider.CreateScope();
            AppDbContext appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDb.Database.SetConnectionString(newConnectionString);
            if ((await appDb.Database.GetPendingMigrationsAsync()).Any())
            {
                activity?.AddEvent(new("Starting db migration"));
                await appDb.Database.MigrateAsync();
                activity?.AddEvent(new("Finished db migration"));
            }
        }
        catch (Exception ex)
        {
            // TODO: Handle error during db creation.
            throw new Exception(ex.Message);
        }
        
        return newConnectionString;
    }
    
    
}
