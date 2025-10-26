using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.MuliTenant;

public interface ICurrentTenantService
{
    TenantId? CurrentTenantId { get; }
    string? CurrentTenantConnectionString { get; }
	
    public Task SetTenant(TenantId id);

    Task<Tenant?> GetTenantByDomain(string domain);
}

public class CurrentTenantService(TenantDbContext db, ILogger<CurrentTenantService> logger): ICurrentTenantService
{
    private Tenant? _tenant;

    public TenantId? CurrentTenantId => _tenant?.Id;
    public string? CurrentTenantConnectionString => _tenant?.ConnectionString;

    public async Task SetTenant(TenantId id)
    {
        var tenant = await db.Tenants.FindAsync(id);
        if(tenant is null)
            throw new Exception("Tenant not found");

        _tenant = tenant;
        logger.LogInformation("Tenant set to: {tenant}", tenant.Name);
    }

    public async Task<Tenant?> GetTenantByDomain(string domain)
    {
        var tenant = await db.Tenants.FirstOrDefaultAsync(t => t.Domain == domain);
        if (tenant is null)
            return null;

        return tenant;
    }
}
