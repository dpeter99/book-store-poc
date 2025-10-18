using BookStore.ApiService.MuliTenant;

namespace BookStore.ApiService.Infrastructure.MuliTenant;

public class TenantResolverMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Logic to resolve tenant from the request
        var tenantDomain = context.Request.Host.Host.Split('.');
        if (tenantDomain.Length == 2)
        {
            await tenantService.SetTenantByDomain(tenantDomain[0]);
        }
        
        await next(context);
    }
}