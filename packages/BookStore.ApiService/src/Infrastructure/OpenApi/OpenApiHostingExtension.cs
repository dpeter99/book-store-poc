using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BookStore.ApiService;

public static class OpenApiHostingExtension
{

	public static IServiceCollection AddVersionedOpenApi(
		this IServiceCollection services,
		IEnumerable<(string group, ApiVersion version)> versions,
		Action<OpenApiOptions>? configure)
	{
		services
			.AddApiVersioning()
			.AddApiExplorer(o =>
			{
				o.SubstituteApiVersionInUrl = true;
				o.FormatGroupName = (name, version) => string.IsNullOrWhiteSpace(name) ? version : $"{name}-{version}";
				o.GroupNameFormat = "'v'VVV";
			});
		
		foreach (var version in versions)
		{
			var versionStr = $"v{version.version:VVV}"; 
			var name = string.IsNullOrWhiteSpace(version.group) ? versionStr : $"{version.group}-{versionStr}";
			
			services.AddOpenApi(name,
				options =>
				{
					if(configure != null)
						configure(options);
					
					options.AddDocumentTransformer((document, context, arg3) =>
					{
						document.Info.Version = versionStr;
						return Task.CompletedTask;
					});
				});
		}
		
		return services;
	}
}
