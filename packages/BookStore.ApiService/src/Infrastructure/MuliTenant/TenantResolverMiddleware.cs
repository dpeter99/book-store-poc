using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.MuliTenant;

namespace BookStore.ApiService.Infrastructure.MuliTenant;

public class TenantResolverMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService)
    {
        // Logic to resolve tenant from the request
        var tenantDomain = context.Request.Host.Host.Split('.');
        if (tenantDomain.Length == 2)
            await TrySetTenant(tenantService, tenantDomain[0]);
        
        await next(context);
    }

    public async Task TrySetTenant(ICurrentTenantService tenantService, string tenantDomain)
    {
        var tenant = await tenantService.GetTenantByDomain(tenantDomain);
        if (tenant is null)
            return;
            
        await tenantService.SetTenant(tenant.Id);
    }
}
