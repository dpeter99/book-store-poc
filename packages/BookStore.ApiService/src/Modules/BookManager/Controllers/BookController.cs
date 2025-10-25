using Asp.Versioning;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.Modules.BookManager.DTO;
using BookStore.ApiService.Modules.BookManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DomainBook = BookStore.ApiService.Modules.BookManager.Model.Book;

namespace BookStore.ApiService.Modules.BookManager.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/book", Name = "Books")]
[ApiVersion("1")]
[Authorize(Policy = "User")]
public class BookController(IBookService bookService) : Controller
{
    
    [HttpGet]
    [EndpointName("get-books")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDTO>))]
    public async Task<Ok<IEnumerable<BookDTO>>> GetBooks()
    {
        // Placeholder for getting books logic
        var books = (await bookService.GetAll())
            .Select(BookDTO.Create);
        
        return TypedResults.Ok(books);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<BookDTO>,NotFound>> GetBookById(long id)
    {
        // Placeholder for getting a book by id logic
        var book = await bookService.GetById(BookId.From(id));
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
        await bookService.AddBook(new DomainBook()
        {
            Id = BookId.Unspecified,
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = AuthorId.From(book.AuthorId)
        });

        return TypedResults.Created();
    }
}
