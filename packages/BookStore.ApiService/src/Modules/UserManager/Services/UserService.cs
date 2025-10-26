using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Users;
using BookStore.ApiService.MuliTenant;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Modules.UserManager.Services;

public interface IUserService
{
	/// <summary>
	/// Get user by username
	/// </summary>
	Task<User?> GetUserByName(string userName);

	/// <summary>
	/// Get all users for the current tenant
	/// </summary>
	Task<IEnumerable<User>> GetAll();

	/// <summary>
	/// Get user by ID
	/// </summary>
	Task<User?> GetById(UserId id);

	/// <summary>
	/// Create a new user
	/// </summary>
	Task<User> CreateUser(string username, string[] roles);

	/// <summary>
	/// Update an existing user
	/// </summary>
	Task<User?> UpdateUser(UserId id, string? username = null, string[]? roles = null);

	/// <summary>
	/// Delete a user
	/// </summary>
	Task<bool> DeleteUser(UserId id);
}

public class UserService(AppDbContext dbContext, ICurrentTenantService tenantService) : IUserService
{
	public async Task<User?> GetUserByName(string userName)
	{
		return await dbContext.Users
			.FirstOrDefaultAsync(u => u.Username == userName);
	}

	public async Task<IEnumerable<User>> GetAll()
	{
		return await dbContext.Users
			.OrderBy(u => u.Username)
			.ToListAsync();
	}

	public async Task<User?> GetById(UserId id)
	{
		return await dbContext.Users
			.FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<User> CreateUser(string username, string[] roles)
	{
		if(tenantService.CurrentTenantId == null)
			throw new Exception("Tenant not found");
		
		Console.WriteLine($"TenantID: {tenantService.CurrentTenantId}");
		
		var user = new User
		{
			Username = username,
			Roles = roles,
			TenantId = tenantService.CurrentTenantId ?? TenantId.Unspecified
		};

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		return user;
	}

	public async Task<User?> UpdateUser(UserId id, string? username = null, string[]? roles = null)
	{
		var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
		{
			return null;
		}

		if (username != null)
		{
			user.Username = username;
		}

		if (roles != null)
		{
			user.Roles = roles;
		}

		await dbContext.SaveChangesAsync();
		return user;
	}

	public async Task<bool> DeleteUser(UserId id)
	{
		var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
		if (user == null)
		{
			return false;
		}

		dbContext.Users.Remove(user);
		await dbContext.SaveChangesAsync();
		return true;
	}
}
