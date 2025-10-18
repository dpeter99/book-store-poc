using System.ComponentModel.DataAnnotations;

namespace BookStore.ApiService.Database.Entities;

public class Tenant
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Domain { get; set; }
}