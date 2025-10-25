using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Users;
using BookStore.ApiService.Modules.UserManager.Services;
using BookStore.ApiService.MuliTenant;
using BookStore.ApiService.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.ApiService.Tests.Modules.UserManager;

public class UserServiceIntegrationTests(ApiServiceFixture fixture) : IClassFixture<ApiServiceFixture>
{
	[Fact]
	public async Task CreateUser_ShouldCreateUserSuccessfully()
	{
		// Arrange
		var userService = fixture.Services.GetRequiredService<IUserService>();
		var username = "testuser";
		var roles = new[] { "Admin", "User" };

		// Act
		var createdUser = await userService.CreateUser(username, roles);

		// Assert
		Assert.NotNull(createdUser);
		Assert.NotEqual(UserId.Unspecified, createdUser.Id);
		Assert.Equal(username, createdUser.Username);
		Assert.Equal(roles, createdUser.Roles);

		// Verify user can be retrieved
		var retrievedUser = await userService.GetById(createdUser.Id);
		Assert.NotNull(retrievedUser);
		Assert.Equal(username, retrievedUser.Username);
		Assert.Equal(roles, retrievedUser.Roles);
	}
}
