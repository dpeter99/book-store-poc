using BookStore.ApiService.Modules.BookManager.DTO;
using BookStore.ApiService.Modules.BookManager.Model;
using BookStore.ApiService.Modules.BookManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ApiService.Modules.BookManager.Controllers;

[ApiController]
[Route("api/book", Name = "Books")]
public class BookController(BookService bookService) : Controller
{
    
    [HttpGet]
    public IActionResult GetBooks()
    {
        // Placeholder for getting books logic
        return Ok(bookService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        // Placeholder for getting a book by id logic
        var book = bookService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] CreateBookDTO book)
    {
        // Placeholder for adding a new book logic
        bookService.AddBook(new Book()
        {
            Id = Guid.NewGuid(),
            Title = book.Title,
            Genre = book.Genre,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId
        });
        
        return CreatedAtAction(nameof(GetBookById), new { id = 1 }, book);
    }
}