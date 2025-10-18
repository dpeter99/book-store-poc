using System.ComponentModel.DataAnnotations;

namespace BookStore.ApiService.Database.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    
    public string[] Roles { get; set; } = null!;
    
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}