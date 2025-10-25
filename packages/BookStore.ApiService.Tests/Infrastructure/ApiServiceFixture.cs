using BookStore.ApiService.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace BookStore.ApiService.Tests.Infrastructure;

public class ApiServiceFixture : IDisposable, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private WebApplicationFactory<Program> _factory = default!;

    public IServiceProvider Services = null!;
    
    public ApiServiceFixture()
    {
        // Start PostgreSQL container
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("bookstore_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public void Dispose()
    {
        
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine("Initializing database...");
        await _dbContainer.StartAsync();
        
        var connectionString = _dbContainer.GetConnectionString();
        Console.WriteLine($"Connection string: {connectionString}");
        _factory = new TestWebApplicationFactory(connectionString);

        Services = _factory.Services;
        
        await using var scope = _factory.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

file sealed class TestWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
    }

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
