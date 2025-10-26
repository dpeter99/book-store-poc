using Asp.Versioning;
using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Infrastructure.MuliTenant;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ApiService.Modules.Core.Controllers;

[ApiController]
[ApiVersionNeutral]
[Route("api/tenant")]
public class TenantController(ITenantService service)
{
    [HttpPost]
    public async Task<Ok<Tenant>> CreateTenant([FromBody] CreateTenantData tenant)
    {
        var res = await service.Create(tenant);
        return TypedResults.Ok(res);
    }
}
