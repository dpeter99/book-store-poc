namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for creating a new user (Internal API)
/// </summary>
public class CreateUserDTO
{
	public required string Username { get; set; }
	public string[] Roles { get; set; } = [];
}