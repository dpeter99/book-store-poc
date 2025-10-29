using BookStore.ApiService.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BookStore.ApiService;

internal sealed class AuthSchemaTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
	public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
	{
		var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
		if (authenticationSchemes.Any(authScheme => authScheme.Name == DummyAuthenticationHandler.AuthenticationScheme))
		{
			var requirements = new Dictionary<string, OpenApiSecurityScheme>
			{
				[DummyAuthenticationHandler.AuthenticationScheme] = new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					Scheme = "bearer", // "bearer" refers to the header name here
					In = ParameterLocation.Header,
					BearerFormat = "Login"
				}
			};
			document.Components ??= new OpenApiComponents();
			document.Components.SecuritySchemes = requirements;
			
			// Apply it as a requirement for all operations
			foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
			{
				operation.Value.Security.Add(new OpenApiSecurityRequirement
				{
					[new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = DummyAuthenticationHandler.AuthenticationScheme, Type = ReferenceType.SecurityScheme } }] = Array.Empty<string>()
				});
			}
		}
	}
}

public static class AuthSchemaTransformerExtensions
{
	public static OpenApiOptions AddAuth(this OpenApiOptions options)
	{
		options.AddDocumentTransformer<AuthSchemaTransformer>();
		return options;
	}
}
