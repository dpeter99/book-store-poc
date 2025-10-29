using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Vogen;

namespace BookStore.ApiService;

public class BrandedTypeSchemaTransformer : IOpenApiSchemaTransformer
{
	public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
	{
		var attribute = FindAttribute(context);
		if(attribute != null)
		{
			schema.Format = $"brand::{attribute.Brand}";
		}
		return Task.CompletedTask;
	}

	private static BrandedTypeAttribute? FindAttribute(OpenApiSchemaTransformerContext context)
	{
		var attribute = context.JsonPropertyInfo?.AttributeProvider?
			.GetCustomAttributes(typeof(BrandedTypeAttribute), true)
			.OfType<BrandedTypeAttribute>().FirstOrDefault();
		if (attribute != null)
		{
			return attribute;
		}
		attribute = context.JsonTypeInfo.Type
			.GetCustomAttributes(typeof(BrandedTypeAttribute), false)
			.OfType<BrandedTypeAttribute>().FirstOrDefault();
		return attribute;
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
