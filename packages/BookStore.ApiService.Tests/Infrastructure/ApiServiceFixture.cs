using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace BookStore.ApiService.Tests.Infrastructure;

public class ApiServiceFixture : IDisposable, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private WebApplicationFactory<Program> _factory = default!;

    public IServiceProvider apiHost = null!;
    
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

        apiHost = _factory.Services;
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
        builder.UseEnvironment("Testing");
        
        builder.ConfigureAppConfiguration(
            cb => cb.AddInMemoryCollection(
                new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
                {
                    ["DbContextOptions:ConnectionString"] = connectionString,
                }
            )
        );
    }

    protected override TestServer CreateServer(IWebHostBuilder builder)
    {
        _ = builder.UseEnvironment("Testing");
        
        return base.CreateServer(builder);
    }
}
