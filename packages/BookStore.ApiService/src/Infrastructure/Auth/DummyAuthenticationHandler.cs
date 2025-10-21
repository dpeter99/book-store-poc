using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BookStore.ApiService.Database;
using BookStore.ApiService.Modules;
using BookStore.ApiService.MuliTenant;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace BookStore.ApiService.Infrastructure.Auth;

public sealed class DummyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public bool UseCookiesBackup { get; set; }
}

public sealed class DummyAuthenticationHandler(
    IOptionsMonitor<DummyAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IUserService userService
) : AuthenticationHandler<DummyAuthenticationSchemeOptions>(
    options,
    logger,
    encoder
)
{
    public const string AuthenticationScheme = "DummyLogin";
    public const string HeaderName = "Authorization";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = Authenticate();
        if (result.Succeeded)
            return Task.FromResult(result);

        return Task.FromResult(AuthenticateResult.NoResult());
    }

    private AuthenticateResult Authenticate()
    {
        var loginString = Context.Request.Headers[HeaderName].FirstOrDefault();
        if (loginString is null)
            return AuthenticateResult.NoResult();

        var loginValue = DecodeUserIdAndPassword(loginString);
        
        if(loginValue.userid is null || loginValue.password is null)
            return AuthenticateResult.Fail("Invalid Authorization Header");
        
        Logger.LogInformation("Authenticate User: {loginValue}", loginValue);
        
        var user = userService.GetUserByName(loginValue.userid);
        if(user is null)
            return AuthenticateResult.Fail("User not found");

        Logger.LogInformation("Authenticated User is Id: {id}", user.Id);
        
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        ];

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var userTenantId = user.TenantId;
        if (userTenantId is not null)
            claims.Add(new Claim(Claims.TenantId, userTenantId.Value.Value.ToString()));
        
        return AuthenticateResult.Success(
            new AuthenticationTicket(
                new ClaimsPrincipal([
                    new ClaimsIdentity(claims, AuthenticationScheme)
                ]),
                AuthenticationScheme
            )
        );
    }
    
    private static (string? userid, string? password) DecodeUserIdAndPassword(string encodedAuth)
    {
        var parts = encodedAuth.Split(':');
        if (parts.Length <= 1)
            return (null, null);
        
        return (parts[0], parts[1]);
    }
}