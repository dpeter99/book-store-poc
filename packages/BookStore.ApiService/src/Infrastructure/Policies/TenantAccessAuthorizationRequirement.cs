using System.Security.Claims;
using BookStore.ApiService.Infrastructure.Auth;
using BookStore.ApiService.MuliTenant;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.ApiService.Infrastructure.Policies;

public class TenantAccessAuthorizationRequirement
    : IAuthorizationRequirement
{
}

public class TenantAccessAuthorizationRequirementHandler(ITenantService tenantService) : AuthorizationHandler<TenantAccessAuthorizationRequirement>
{
    
    protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TenantAccessAuthorizationRequirement requirement)
        {
            if (context.User is not null)
            {
                var tenantClaim = context.User.FindFirst(Claims.TenantId);
                if (tenantClaim is null)
                {
                    return Task.CompletedTask;
                }

                if (tenantService.CurrentTenantId.ToString() == tenantClaim.Value)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
}