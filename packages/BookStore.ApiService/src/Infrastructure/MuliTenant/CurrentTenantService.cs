using BookStore.ApiService.Infrastructure.MuliTenant;

namespace BookStore.ApiService.MuliTenant;

public interface ICurrentTenantService
{
    Guid? TenantId { get; }
    
    public Task<bool> SetTenantByDomain(string domain);
}

public class CurrentTenantServiceImpl(ITenantService tenantService): ICurrentTenantService
{
    private Tenant? _tenant;
    
    public Guid? TenantId => _tenant?.Id;

    public async Task<bool> SetTenantByDomain(string domain)
    {
        // Check db for validity
        var tenant = await tenantService.GetTenantByDomain(domain);
        if(tenant is null)
            return false;
        
        _tenant = tenant;
        return true;
    }
}