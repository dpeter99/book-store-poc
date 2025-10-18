using BookStore.ApiService.Database;
using BookStore.ApiService.MuliTenant;
using Microsoft.AspNetCore.Authentication;

namespace BookStore.ApiService.Modules;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string[] Roles { get; set; } = null!;
    
    public Guid? TenantId { get; set; }
}

public interface IUserService
{
    User? GetUserByName(string userName);
}

public class UserService(AppDbContext dbContext) : IUserService
{
    public User? GetUserByName(string userName)
    {
        var userEntity = dbContext.Users.FirstOrDefault(u=>u.Username == userName);
        if (userEntity != null)
        {
            return new User
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Roles = userEntity.Roles,
                TenantId = userEntity.TenantId
            };
        }

        return null;
    }
}