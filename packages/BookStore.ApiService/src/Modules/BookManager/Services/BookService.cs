using BookStore.ApiService.Database;
using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.MuliTenant;
using Microsoft.EntityFrameworkCore;
using DomainBook = BookStore.ApiService.Modules.BookManager.Model.Book;
using DbBook = BookStore.ApiService.Database.Entities.Modules.Books.Book;

namespace BookStore.ApiService.Modules.BookManager.Services;

public interface IBookService
{
    Task<IEnumerable<DomainBook>> GetAll();
    Task<DomainBook?> GetById(BookId id);
    Task AddBook(DomainBook book);
}

public class BookService(AppDbContext dbContext, ICurrentTenantService tenantService) : IBookService
{
    public async Task<IEnumerable<DomainBook>> GetAll()
    {
        var books = await dbContext.Books
            .Select(b => new DomainBook()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedDate = b.PublishedDate,
                    AuthorId = b.Author.Id
                }
            )
            .ToListAsync();
        return books;
    }

    public async Task<DomainBook?> GetById(BookId id)
    {
        var bookEntity = await dbContext.Books
	        .Include(book => book.AuthorId)
	        .FirstOrDefaultAsync(b => b.Id == id);
        if (bookEntity == null)
        {
            return null;
        }
        return new DomainBook()
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Genre = bookEntity.Genre,
            PublishedDate = bookEntity.PublishedDate,
            AuthorId = bookEntity.AuthorId
        };
    }

    public async Task AddBook(DomainBook book)
    {

        // Logic to add book to the database
        dbContext.Books.Add(new DbBook()
        {
            Id = book.Id,
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId,
            TenantId = tenantService.CurrentTenantId ?? TenantId.Unspecified //TODO figure out a better way to do this
        });
        await dbContext.SaveChangesAsync();
    }
}
