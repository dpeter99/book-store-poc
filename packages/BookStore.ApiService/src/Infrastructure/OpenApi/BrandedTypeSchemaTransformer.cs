using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace BookStore.ApiService;

public class BrandedTypeSchemaTransformer : IOpenApiSchemaTransformer
{
	public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
	{
		var customAttributeProvider = context.JsonPropertyInfo?.AttributeProvider;
		if(customAttributeProvider != null && customAttributeProvider.IsDefined(typeof(BrandedTypeAttribute), false))
		{
			var attribute = customAttributeProvider.GetCustomAttributes(typeof(BrandedTypeAttribute), false)
				.OfType<BrandedTypeAttribute>().First();
			schema.Format = $"brand::{attribute.Brand}";
		}
		return Task.CompletedTask;
	}
}

public static class BrandedTypeAttributeExtensions
{
	public static OpenApiOptions AddBrandedTypes(this OpenApiOptions options)
	{
		options.AddSchemaTransformer<BrandedTypeSchemaTransformer>();
		return options;
	}
}
