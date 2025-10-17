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
    public IActionResult GetBookById(int id)
    {
        // Placeholder for getting a book by ID logic
        return Ok($"Book{id}");
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] object book)
    {
        // Placeholder for adding a new book logic
        bookService.AddBook(new Book()
        {
            Id = Guid.NewGuid(),
            Title = "New Book",
            Genre = "Genre",
            PublishedDate = DateTime.Now,
            AuthorId = Guid.NewGuid()
        });
        
        return CreatedAtAction(nameof(GetBookById), new { id = 1 }, book);
    }
}