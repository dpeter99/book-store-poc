using BookStore.ApiService.Modules.BookManager.DTO;
using BookStore.ApiService.Modules.BookManager.Model;
using BookStore.ApiService.Modules.BookManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ApiService.Modules.BookManager.Controllers;

[ApiController]
[Route("api/book", Name = "Books")]
[Authorize(Roles = "admin")]
public class BookController(BookService bookService) : Controller
{
    
    [HttpGet]
    public async Task<Ok<IEnumerable<BookDTO>>> GetBooks()
    {
        // Placeholder for getting books logic
        var books = (await bookService.GetAll())
            .Select(BookDTO.Create);
        
        return TypedResults.Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<Results<Ok<BookDTO>,NotFound>> GetBookById(Guid id)
    {
        // Placeholder for getting a book by id logic
        var book = await bookService.GetById(id);
        if (book == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(BookDTO.Create(book));
    }

    [HttpPost]
    public async Task<Created> AddBook([FromBody] CreateBookDTO book)
    {
        // Placeholder for adding a new book logic
        await bookService.AddBook(new Book()
        {
            Id = Guid.NewGuid(),
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId
        });

        return TypedResults.Created();
    }
}