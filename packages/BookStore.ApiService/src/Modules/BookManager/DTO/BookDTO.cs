using BookStore.ApiService.Modules.BookManager.Model;

namespace BookStore.ApiService.Modules.BookManager.DTO;

public class BookDTO
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }

    public static BookDTO Create(Book book)
    {
        return new()
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId
        };
    }
}