using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace BookStore.ApiService.Infrastructure.Auth;

public static class AuthenticationStartupExtensions
{
	public static void AddDummyAuthentication(
		this IServiceCollection services,
		string? domain,
		string? clientId
	)
	{
		// var authBuilder = services
		// 	.AddAuthentication(o => { })
			
	}
}