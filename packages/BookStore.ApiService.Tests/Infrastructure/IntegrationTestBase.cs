using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace BookStore.ApiService.Tests.Infrastructure;

/// <summary>
/// Base class for integration tests using WebApplicationFactory, Testcontainers, and Respawn
/// </summary>
public class IntegrationTestBase : IAsyncLifetime
{
	private PostgreSqlContainer? _dbContainer;
	private NpgsqlConnection? _connection;
	private Respawner? _respawner;
	private WebApplicationFactory<Program>? _factory;
	protected string ConnectionString { get; private set; } = string.Empty;

	public async Task InitializeAsync()
	{
		// Start PostgreSQL container
		_dbContainer = new PostgreSqlBuilder()
			.WithImage("postgres:16")
			.WithDatabase("bookstore_test")
			.WithUsername("postgres")
			.WithPassword("postgres")
			.Build();

		await _dbContainer.StartAsync();

		ConnectionString = _dbContainer.GetConnectionString();

		// Create WebApplicationFactory with overridden connection string
		_factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					// Remove existing DbContext registrations
					services.RemoveAll<DbContextOptions<AppDbContext>>();
					services.RemoveAll<AppDbContext>();
					services.RemoveAll<DbContextOptions<TenantDbContext>>();
					services.RemoveAll<TenantDbContext>();

					// Add DbContexts with test database connection string
					services.AddDbContext<AppDbContext>(options =>
						options.UseNpgsql(ConnectionString));

					services.AddDbContext<TenantDbContext>(options =>
						options.UseNpgsql(ConnectionString));
				});

				// Set environment to Development to avoid Aspire issues
				builder.UseEnvironment("Development");

				// Override configuration
				builder.UseSetting("ConnectionStrings:bookdb", ConnectionString);
			});

		// Run migrations to set up the database schema
		using (var scope = _factory.Services.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			await dbContext.Database.MigrateAsync();
		}

		// Set up Respawn
		_connection = new NpgsqlConnection(ConnectionString);
		await _connection.OpenAsync();

		_respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
		{
			DbAdapter = DbAdapter.Postgres,
			SchemasToInclude = ["public"],
			TablesToIgnore = ["__EFMigrationsHistory"]
		});
	}

	public async Task DisposeAsync()
	{
		if (_connection != null)
		{
			await _connection.CloseAsync();
			await _connection.DisposeAsync();
		}

		if (_factory != null)
		{
			await _factory.DisposeAsync();
		}

		if (_dbContainer != null)
		{
			await _dbContainer.DisposeAsync();
		}
	}

	/// <summary>
	/// Resets the database to a clean state using Respawn
	/// </summary>
	protected async Task ResetDatabaseAsync()
	{
		if (_respawner != null && _connection != null)
		{
			await _respawner.ResetAsync(_connection);
		}
	}

	/// <summary>
	/// Gets a scoped service from the application
	/// </summary>
	protected T GetService<T>() where T : notnull
	{
		if (_factory == null)
			throw new InvalidOperationException("Factory not initialized");

		var scope = _factory.Services.CreateScope();
		return scope.ServiceProvider.GetRequiredService<T>();
	}

	/// <summary>
	/// Creates a new service scope for accessing scoped services
	/// </summary>
	protected IServiceScope CreateScope()
	{
		if (_factory == null)
			throw new InvalidOperationException("Factory not initialized");

		return _factory.Services.CreateScope();
	}

	/// <summary>
	/// Seeds a test tenant into the database and returns its ID
	/// </summary>
	protected async Task<TenantId> SeedTestTenantAsync(string name = "TestTenant", string domain = "test.local")
	{
		await using var connection = new NpgsqlConnection(ConnectionString);
		await connection.OpenAsync();

		await using var command = new NpgsqlCommand(
			"INSERT INTO \"Tenants\" (\"Name\", \"Domain\") VALUES (@name, @domain) RETURNING \"Id\"",
			connection);

		command.Parameters.AddWithValue("name", name);
		command.Parameters.AddWithValue("domain", domain);

		var result = await command.ExecuteScalarAsync();
		var tenantId = result != null ? Convert.ToInt64(result) : 0;

		return TenantId.From(tenantId);
	}

	/// <summary>
	/// Creates an HttpClient for making requests to the test server
	/// </summary>
	protected HttpClient CreateClient()
	{
		if (_factory == null)
			throw new InvalidOperationException("Factory not initialized");

		return _factory.CreateClient();
	}
}