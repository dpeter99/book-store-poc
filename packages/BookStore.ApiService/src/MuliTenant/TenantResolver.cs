using BookStore.ApiService.MuliTenant;

namespace BookStore.ApiService.Database.Entities;

public class TenantResolver(RequestDelegate next, ILogger<TenantResolver> logger)
{
    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService)
    {
        // Logic to resolve tenant from the request
        var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
        if (!string.IsNullOrEmpty(tenantId))
        {
            logger.LogInformation($"TenantId: {tenantId}");
            await tenantService.SetTenantId(new  Guid(tenantId));
        }
        
        await next(context);
    }
}