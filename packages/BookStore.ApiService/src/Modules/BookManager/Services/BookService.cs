using BookStore.ApiService.Modules.BookManager.Model;

namespace BookStore.ApiService.Modules.BookManager.Services;

public class BookService
{
    private List<Book> _books = [
        new Book
        {
            Id = 1,
            Title = "Sample Book 1",
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 1
        },
        new Book
        {
            Id = 2,
            Title = "Sample Book 2",
            Genre = "Non-Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 2
        }
    ];
    
    public IEnumerable<Book> GetAll()
    {
        return _books;
    }
    
    Book GetById(int id)
    {
        return new Book
        {
            Id = id,
            Title = "Sample Book",
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = 1
        };
    }
}