using System.ComponentModel.DataAnnotations;

namespace BookStore.ApiService.Database.Entities.Modules.Books;

public class Book
{
    [Key]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
}