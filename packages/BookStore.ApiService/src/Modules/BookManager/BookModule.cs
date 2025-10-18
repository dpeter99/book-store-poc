using BookStore.ApiService.Modules.BookManager.Services;

namespace BookStore.ApiService.Modules.BookManager;

public static class BookModule
{
    public static void AddBookModule(this IHostApplicationBuilder builder)
    {
        // Placeholder for module configuration logic

        builder.Services.AddScoped<IBookService,BookService>();
    }
}