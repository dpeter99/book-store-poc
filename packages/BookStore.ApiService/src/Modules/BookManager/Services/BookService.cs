using BookStore.ApiService.Database;
using BookStore.ApiService.Modules.BookManager.Model;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Modules.BookManager.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book?> GetById(Guid id);
    Task AddBook(Book book);
}

public class BookService(AppDbContext dbContext, ITenantService tenantService) : IBookService
{
    public async Task<IEnumerable<Book>> GetAll()
    {
        var books = await dbContext.Books
            .Select(b => new Book()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedDate = b.PublishedDate,
                    AuthorId = b.AuthorId
                }
            )
            .ToListAsync();
        return books;
    }
    
    public async Task<Book?> GetById(Guid id)
    {
        var bookEntity = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (bookEntity == null)
        {
            return null;
        }
        return new Book()
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Genre = bookEntity.Genre,
            PublishedDate = bookEntity.PublishedDate,
            AuthorId = bookEntity.AuthorId
        };
    }
    
    public async Task AddBook(Book book)
    {
        
        // Logic to add book to the database
        dbContext.Books.Add(new Database.Entities.Modules.Books.Book()
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId,
            TenantId = tenantService.CurrentTenantId ?? Guid.Empty //TODO figure out a better way to do this
        });
        await dbContext.SaveChangesAsync();
    }
}