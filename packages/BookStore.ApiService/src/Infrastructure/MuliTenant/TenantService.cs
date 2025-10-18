using BookStore.ApiService.Database;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Infrastructure.MuliTenant;

public class Tenant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Domain { get; set; }
}

public interface ITenantService
{
    Task<Tenant?> GetTenantByDomain(string domain);
}

public class TenantService(TenantDbContext db): ITenantService
{
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