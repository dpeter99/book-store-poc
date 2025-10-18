namespace BookStore.ApiService.Modules.BookManager.Model;

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
}