using BookStore.ApiService.Database.Entities;
using BookStore.ApiService.Database.Entities.Modules.Users;
using Riok.Mapperly.Abstractions;

namespace BookStore.ApiService.Modules.UserManager.DTO;

/// <summary>
/// DTO for creating a new user (Internal API)
/// </summary>
public partial class CreateUserDTO
{
	public required string Username { get; set; }
	public string[] Roles { get; set; } = [];

	[Mapper]
	public static partial class Mappings
	{
		public static User ToEntity(CreateUserDTO dto, TenantId TenantId)
		{
			return new()
			{
				Username = dto.Username,
				Roles = dto.Roles,
				TenantId = TenantId
			};
		}
	}
}
