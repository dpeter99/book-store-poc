using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Users;
using Riok.Mapperly.Abstractions;

namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for returning user information (Internal API)
/// </summary>
public partial class UserDTO
{
	public UserId Id { get; set; }
	public required string Username { get; set; }
	public string[] Roles { get; set; } = [];
	public TenantId TenantId { get; set; }

	[Mapper]
	public static partial class Mappings
	{
		/// <summary>
		/// Maps User entity to UserDTO
		/// </summary>
		[MapperIgnoreSource(nameof(User.Tenant))]
		public static partial UserDTO FromEntity(User user);
	}
}