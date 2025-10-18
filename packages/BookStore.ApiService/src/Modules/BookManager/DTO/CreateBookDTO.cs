namespace BookStore.ApiService.Modules.BookManager.DTO;

public class CreateBookDTO
{
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
}