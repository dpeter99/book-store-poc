using BookStore.ApiService.Database.Entities.Modules.Users;
using Riok.Mapperly.Abstractions;

namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for updating an existing user (Internal API)
/// </summary>
public partial class UpdateUserDTO
{
	public string? Username { get; set; }
	public string[]? Roles { get; set; }

	[Mapper]
	public static partial class Mappings
	{
		/// <summary>
		/// Updates an existing User entity from UpdateUserDTO
		/// Only updates non-null properties from the DTO
		/// </summary>
		[MapperIgnoreTarget(nameof(User.Id))]
		[MapperIgnoreTarget(nameof(User.TenantId))]
		[MapperIgnoreTarget(nameof(User.Tenant))]
		public static partial void ApplyTo(UpdateUserDTO dto, User user);
	}
}