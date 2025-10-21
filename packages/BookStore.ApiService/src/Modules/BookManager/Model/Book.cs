using BookStore.ApiService.Database.Entities.Modules.Books;

namespace BookStore.ApiService.Modules.BookManager.Model;

public class Book
{
    public BookId Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public AuthorId AuthorId { get; set; }
}
