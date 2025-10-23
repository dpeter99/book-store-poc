using BookStore.ApiService.Database.Entities.Modules.Users;

namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for returning user information (Internal API)
/// </summary>
public class UserDTO
{
	public long Id { get; set; }
	public required string Username { get; set; }
	public string[] Roles { get; set; } = [];
	public long TenantId { get; set; }

	public static UserDTO Create(User user)
	{
		return new UserDTO
		{
			Id = user.Id.Value,
			Username = user.Username,
			Roles = user.Roles,
			TenantId = user.TenantId.Value
		};
	}
}