using Asp.Versioning;
using BookStore.ApiService.Database.Entities.Modules.Books;
using BookStore.ApiService.Modules.BookManager.DTO;
using BookStore.ApiService.Modules.BookManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ApiService.Modules.BookManager.PublicApi;

[ApiController]
[ApiVersion("2")]
[EndpointGroupName("public")]
[Route("public-api/v{version:apiVersion}/book")]
[Authorize(Policy = "User")]
public class BookController(IBookService bookService) : Controller
{
    
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDTO>))]
	[ApiVersion("1")]
	[ApiVersion("2")]
	public async Task<Ok<IEnumerable<BookDTO>>> GetBooks()
	{
		// Placeholder for getting books logic
		var books = (await bookService.GetAll())
			.Select(BookDTO.Create);
        
		return TypedResults.Ok(books);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDTO))]
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
}
