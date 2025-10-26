using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;

namespace BookStore.ApiService.Database.Entities.Modules.Books;

[ValueObject<long>(conversions: Conversions.EfCoreValueConverter | Conversions.SystemTextJson)]
[Instance("Unspecified", 0)]
public readonly partial record struct AuthorId;

public class Author
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public required AuthorId Id { get; set; }

	[Required]
	[MaxLength(200)]
	public required string Name { get; set; }

	public required DateTime Birthday { get; set; }

	// Navigation property
	public ICollection<Book> Books { get; set; } = new List<Book>();
}

public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
	public void Configure(EntityTypeBuilder<Author> builder)
	{
		// Configure typed ID conversion using Vogen helper
		builder.Property(a => a.Id)
			.HasVogenConversion()
			.ValueGeneratedOnAdd();
	}
}
