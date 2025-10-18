using BookStore.ApiService.MuliTenant;

namespace BookStore.ApiService.Database.Entities;

public class TenantResolverMiddleware(RequestDelegate next, ILogger<TenantResolverMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Logic to resolve tenant from the request
        var tenantDomain = context.Request.Host.Host.Split('.')[0];
        if (!string.IsNullOrEmpty(tenantDomain))
        {
            logger.LogInformation("Domain is: {tenantDomain}", tenantDomain);
            await tenantService.SetTenantByDomain(tenantDomain);
        }
        
        await next(context);
    }
}