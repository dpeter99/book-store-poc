namespace BookStore.ApiService.Modules.BookManager.Model;

public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
}