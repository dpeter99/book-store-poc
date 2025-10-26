using BookStore.ApiService.Infrastructure.Policies;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.ApiService.Infrastructure.Auth;

public static class ApplicationBuilderTenantExtensions
{
    public static IHostApplicationBuilder ConfigureTenants(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, TenantAccessAuthorizationRequirementHandler>();

        return builder;
    }
}
