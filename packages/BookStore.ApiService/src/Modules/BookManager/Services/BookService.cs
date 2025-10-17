using BookStore.ApiService.Database;
using BookStore.ApiService.Modules.BookManager.Model;

namespace BookStore.ApiService.Modules.BookManager.Services;

public class BookService(AppDbContext dbContext)
{
    public IEnumerable<Book> GetAll()
    {
        var books = dbContext.Books.ToList().Select(b => new Book()
        {
            Id = b.Id,
            Title = b.Title,
            Genre = b.Genre,
            PublishedDate = b.PublishedDate,
            AuthorId = b.AuthorId
        }).ToList();
        return books;
    }
    
    public Book GetById(Guid id)
    {
        return new Book
        {
            Id = id,
            Title = "Sample Book",
            Genre = "Fiction",
            PublishedDate = DateTime.Now,
            AuthorId = new Guid()
        };
    }
    
    public void AddBook(Book book)
    {
        
        // Logic to add book to the database
        dbContext.Books.Add(new Database.Entities.Modules.Books.Book()
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId
        });
        dbContext.SaveChanges();
    }
}