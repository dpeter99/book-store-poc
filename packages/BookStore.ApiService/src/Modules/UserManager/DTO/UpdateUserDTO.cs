namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for updating an existing user (Internal API)
/// </summary>
public class UpdateUserDTO
{
	public string? Username { get; set; }
	public string[]? Roles { get; set; }
}