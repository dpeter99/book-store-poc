using BookStore.ApiService.Database.Entities.Modules.Books;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ApiService.Database;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}