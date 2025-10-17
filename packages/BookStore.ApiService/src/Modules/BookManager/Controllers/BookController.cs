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
}