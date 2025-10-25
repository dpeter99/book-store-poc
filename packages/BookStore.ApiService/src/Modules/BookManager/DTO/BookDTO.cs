using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.Modules.BookManager.Model;
using Riok.Mapperly.Abstractions;

namespace BookStore.ApiService.Modules.BookManager.DTO;

public partial class BookDTO
{
    public BookId Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public AuthorId AuthorId { get; set; }

	[Mapper]
	public static partial class Mappings
	{
		/// <summary>
		/// Maps domain Book model to BookDTO
		/// </summary>
		public static partial BookDTO FromModel(Model.Book book);
	}
}
