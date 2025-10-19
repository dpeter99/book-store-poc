using System.Reflection;
using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace BookStore.ApiService;

public static class OpenApiHostingExtension
{

	public static IServiceCollection AddVersionedOpenApi(
		this IServiceCollection services,
		IEnumerable<(string group, ApiVersion version)> versions
		)
	{
		services
			.AddApiVersioning()
			.AddApiExplorer(o =>
			{
				o.SubstituteApiVersionInUrl = true;
				o.FormatGroupName = (name, version) => $"{name}-{version}";
				o.GroupNameFormat = "'v'VVV";
			});
		
		foreach (var version in versions)
		{
			var versionStr = $"v{version.version:VVV}"; 
			var name = $"{version.group}-{versionStr}";
			
			Console.WriteLine(name);
			
			services.AddOpenApi(name,
				options =>
				{
					options.AddOperationTransformer((operation, context, arg3) =>
					{
						
						return Task.CompletedTask;
					});
					options.AddDocumentTransformer((document, context, arg3) =>
					{
						document.Info.Version = versionStr;
						// document.Servers = [new (){Url = "http://localhost:5493/api"}];
						return Task.CompletedTask;
					});
					options.ShouldInclude = description =>
					{
						Console.WriteLine($"{description.ActionDescriptor.DisplayName} Group: {description.GroupName}");
						return description.GroupName == name;
					};
				});
		}
		
		return services;
	}
}
