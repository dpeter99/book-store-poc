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
        
        var user = userService.GetOrCreateUser(loginValue.userid);

        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        ];
        
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
        
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
        var separator = encodedAuth.IndexOf(':');
        if (separator == -1)
            return (null, null);

        var parts = encodedAuth.Split(':');
        
        return (parts[0], parts[1]);
    }
}