using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;

namespace BookStore.ApiService.Database.Entities.Modules.Users;

[ValueObject<long>(conversions: Conversions.EfCoreValueConverter)]
[Instance("Unspecified", 0)]
public readonly partial record struct UserId;

public class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public required UserId Id { get; set; }
	public string Username { get; set; } = null!;

	public string[] Roles { get; set; } = null!;

	public required TenantId TenantId { get; set; }
	public Tenant? Tenant { get; set; }
}

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		// Configure typed ID conversion using Vogen helper
		builder.Property(u => u.Id)
			.HasVogenConversion()
			.ValueGeneratedOnAdd();

		// Configure TenantId with conversion
		builder.Property(u => u.TenantId)
			.HasVogenConversion();
	}
}