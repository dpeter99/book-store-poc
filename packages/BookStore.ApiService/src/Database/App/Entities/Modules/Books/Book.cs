using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.ApiService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;

namespace BookStore.ApiService.Database.Entities.Modules.Books;

[ValueObject<long>(conversions: Conversions.EfCoreValueConverter | Conversions.SystemTextJson)]
[Instance("Unspecified", 0)]
[BrandedType(nameof(BookId))]
public readonly partial record struct BookId;

[Index(nameof(TenantId))]
[Index(nameof(AuthorId))]
[Index(nameof(TenantId), nameof(AuthorId))]
public class Book
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public required BookId Id { get; set; }

	public required TenantId TenantId { get; set; }

	[Required]
	[MaxLength(500)]
	public required string Title { get; set; }

	[MaxLength(100)]
	public required string Genre { get; set; }

	public DateTime PublishedDate { get; set; }

	[ForeignKey(nameof(Author))]
	public required AuthorId AuthorId { get; set; }

	public Author Author { get; set; } = null!;

	// Audit fields
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
	public void Configure(EntityTypeBuilder<Book> builder)
	{
		// Configure typed ID conversions using Vogen helper
		builder.Property(b => b.Id)
			.HasVogenConversion()
			.ValueGeneratedOnAdd();

		builder.Property(b => b.TenantId)
			.HasVogenConversion();

		builder.Property(b => b.AuthorId)
			.HasVogenConversion();

		// Configure relationship
		builder.HasOne(b => b.Author)
			.WithMany(a => a.Books)
			.HasForeignKey(b => b.AuthorId)
			.OnDelete(DeleteBehavior.Restrict);

		// Configure default values
		builder.Property(b => b.CreatedAt)
			.HasDefaultValueSql("NOW()");

		// Note: Query filter for TenantId is applied in AppDbContext
		// to have access to CurrentTenantId at runtime
	}
}
