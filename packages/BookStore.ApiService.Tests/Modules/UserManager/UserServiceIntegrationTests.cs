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
		var userService = fixture.apiHost.GetRequiredService<UserService>();
		Console.WriteLine(userService.ToString());
	}
}
