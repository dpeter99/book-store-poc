using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;

namespace BookStore.ApiService.Database.Entities;

[ValueObject<long>(conversions: Conversions.EfCoreValueConverter | Conversions.SystemTextJson)]
[Instance("Unspecified", 0)]
public readonly partial record struct TenantId;

public class Tenant
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public TenantId Id { get; set; }
	public required string Name { get; set; }
	public required string Domain { get; set; }
}

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
{
	public void Configure(EntityTypeBuilder<Tenant> builder)
	{
		// Configure typed ID conversion using Vogen helper
		builder.Property(t => t.Id)
			.HasVogenConversion()
			.ValueGeneratedOnAdd();
	}
}