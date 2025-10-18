namespace BookStore.ApiService.MuliTenant;

public interface ICurrentTenantService
{
    Guid? TenantId { get; }
    
    public Task<bool> SetTenantId(Guid tenantId);
}

public class CurrentTenantServiceImpl: ICurrentTenantService
{
    public Guid? TenantId { get; private set; }

    public Task<bool> SetTenantId(Guid tenantId)
    {
        // Check db for validity
        
        TenantId = tenantId;
        return Task.FromResult(true);
    }
}