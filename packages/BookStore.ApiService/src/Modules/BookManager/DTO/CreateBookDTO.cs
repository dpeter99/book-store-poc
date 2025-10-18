namespace BookStore.ApiService.Modules.BookManager.DTO;

public class CreateBookDTO
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
}