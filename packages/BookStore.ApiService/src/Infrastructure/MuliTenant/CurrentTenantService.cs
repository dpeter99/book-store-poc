using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.MuliTenant;

public class Tenant
{
    public TenantId Id { get; set; }
    public required string Name { get; set; }
    public required string Domain { get; set; }
}

public interface ITenantService
{
    TenantId? CurrentTenantId { get; }
    Tenant? CurrentTenant { get; }

    public Task<bool> SetTenantByDomain(string domain);
    public Task<bool> SetTenant(TenantId id);

    Task<Tenant?> GetTenantByDomain(string domain);

    // TODO: Add creation and other handling
}

public class TenantService(TenantDbContext db, ILogger<TenantService> logger): ITenantService
{
    private Tenant? _tenant;

    public TenantId? CurrentTenantId => _tenant?.Id;
    public Tenant? CurrentTenant => _tenant;


    public async Task<bool> SetTenantByDomain(string domain)
    {
        // Check db for validity
        var tenant = await GetTenantByDomain(domain);
        if(tenant is null)
            return false;
        
        _tenant = tenant;
        logger.LogInformation("Tenant set to: {tenant}", tenant.Name);
        return true;
    }

    public async Task<bool> SetTenant(TenantId id)
    {
        var tenant = await db.Tenants.FindAsync(id);
        if(tenant is null)
            return false;
        
        _tenant = new ()
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Domain = tenant.Domain,
        };
        logger.LogInformation("Tenant set to: {tenant}", tenant.Name);
        return true;
    }

    public async Task<Tenant?> GetTenantByDomain(string domain)
    {
        var tenant = await db.Tenants.FirstOrDefaultAsync(t => t.Domain == domain);
        if (tenant is null)
            return null;

        return new Tenant()
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Domain = tenant.Domain,
        };
    }
}
