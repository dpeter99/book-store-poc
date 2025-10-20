using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.ApiService.Database.Entities.Modules.Books;

public class Author
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public required long Id { get; set; }
	public required string Name { get; set; }
	public required DateTime Birthday { get; set; }
}
