using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStore.AppHost;

public static class OpenApiResourceBuilderExtensions
{
	public static IResourceBuilder<T> WithOpenApiDocs<T>(
		this IResourceBuilder<T> builder,
		string name,
		string displayName,
		string path
		)
	where T : IResourceWithEndpoints
	{
		return builder
			.WithCommand(
				name,
				displayName,
				executeCommand: async _ =>
				{
					try
					{
						var endpoint = builder.GetEndpoint("https");
						var url = $"{endpoint.Url}/{path}";
						Process.Start(new  ProcessStartInfo(url) { UseShellExecute = true });
						return new ExecuteCommandResult { Success = true };
					}
					catch (Exception e)
					{
						return new ExecuteCommandResult { Success = false, ErrorMessage = e.Message };
					}
				},
				new CommandOptions
					{
						IconName = "Document",
						UpdateState = context =>
							context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ?
								ResourceCommandState.Enabled : ResourceCommandState.Disabled,
						
					}
			);
	}
}
