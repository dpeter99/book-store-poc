using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Books;
using Riok.Mapperly.Abstractions;
using DomainBook = BookStore.ApiService.Modules.BookManager.Model.Book;

namespace BookStore.ApiService.Modules.BookManager.DTO;

public partial class CreateBookDTO
{
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public AuthorId AuthorId { get; set; }

	[Mapper]
	public static partial class Mappings
	{
		/// <summary>
		/// Maps CreateBookDTO to domain Book model
		/// Note: Id must be set separately
		/// </summary>
		[MapperIgnoreTarget(nameof(DomainBook.Id))]
		public static partial DomainBook ToModel(CreateBookDTO dto);

		/// <summary>
		/// Maps CreateBookDTO to Book entity with TenantId
		/// Manual mapper to handle required properties
		/// </summary>
		public static Book ToEntity(CreateBookDTO dto, TenantId tenantId)
		{
			return new()
			{
				Id = BookId.Unspecified,
				Title = dto.Title,
				Genre = dto.Genre,
				PublishedDate = dto.PublishedDate,
				AuthorId = dto.AuthorId,
				TenantId = tenantId
			};
		}
	}
}
