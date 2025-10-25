using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace BookStore.ApiService.Tests.Infrastructure;

public class ApiServiceFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("bookstore_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    private WebApplicationFactory<Program> _factory = default!;

    public IServiceProvider Services = null!;
    
    public TenantId tenantId = TenantId.Unspecified;

    public async Task InitializeAsync()
    {
        Console.WriteLine("Initializing database...");
        await _dbContainer.StartAsync();

        var connectionString = _dbContainer.GetConnectionString();
        Console.WriteLine($"Connection string: {connectionString}");
        _factory = new TestWebApplicationFactory(connectionString);

        Services = _factory.Services;

        await using var scope = _factory.Services.CreateAsyncScope();
        await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
        {
            await dbContext.Database.MigrateAsync();
            await dbContext.SaveChangesAsync();
        }

        await using var tenantDbContext = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
        await AddTestTenant(tenantDbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private async Task AddTestTenant(TenantDbContext context)
    {
        var tenant = new Tenant()
        {
            Name = "Test Tenant",
            Domain = "testdomain",
        };
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();

        tenantId = tenant.Id;
        Console.WriteLine($"Tenant Id: {tenantId}");
    }
    
}

file sealed class TestWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureHostConfiguration(cb => cb.AddInMemoryCollection(
                new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
                {
                    ["ConnectionStrings:bookdb"] = connectionString,
                }
            )
        );
        
        return base.CreateHost(builder);
    }
}
