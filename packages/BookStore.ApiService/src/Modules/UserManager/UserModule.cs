using BookStore.ApiService.Modules.UserManager.Services;

namespace BookStore.ApiService.Modules.UserManager;

public static class UserModule
{
	public static void AddUserModule(this IHostApplicationBuilder builder)
	{
		// Register UserService for dependency injection
		builder.Services.AddScoped<IUserService, UserService>();
	}
}