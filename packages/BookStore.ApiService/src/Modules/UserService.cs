using BookStore.ApiService.Database;
using Microsoft.AspNetCore.Authentication;

namespace BookStore.ApiService.Modules;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string[] Roles { get; set; } = null!;
}

public interface IUserService
{
    User GetOrCreateUser(string userName);
}

public class UserService(AppDbContext dbContext) : IUserService
{
    public User GetOrCreateUser(string userName)
    {
        var userEntity = dbContext.Users.FirstOrDefault(u=>u.Username == userName);
        if (userEntity != null)
        {
            return new User
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Roles = userEntity.Roles,
            };
        }

        var newUser = new Database.Entities.User
        {
            Id = Guid.NewGuid(),
            Username = userName,
            Roles = [] // Placeholder for roles
        };

        dbContext.Users.Add(newUser);
        dbContext.SaveChanges();

        return new User
        {
            Id = newUser.Id,
            Username = newUser.Username,
            Roles = newUser.Roles,
        };
    }
}