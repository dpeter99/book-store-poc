using Asp.Versioning;
using BookStore.ApiService.Database.Entities.Modules.Users;
using BookStore.ApiService.Modules.UserManager.DTO;
using BookStore.ApiService.Modules.UserManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ApiService.Modules.UserManager.Controllers;

/// <summary>
/// Internal API controller for user management
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/user")]
[ApiVersion("1")]
[Authorize(Policy = "User")]
public class UserController(IUserService userService) : Controller
{
	/// <summary>
	/// Get all users in the current tenant
	/// </summary>
	[HttpGet]
	[EndpointName("get-users")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDTO>))]
	public async Task<Ok<IEnumerable<UserDTO>>> GetUsers()
	{
		var users = (await userService.GetAll()).Select(UserDTO.Mappings.FromEntity);
		return TypedResults.Ok(users);
	}

	/// <summary>
	/// Get a specific user by ID
	/// </summary>
	[HttpGet("{id}")]
	[EndpointName("get-user-by-id")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<Results<Ok<UserDTO>, NotFound>> GetUserById(long id)
	{
		var user = await userService.GetById(UserId.From(id));
		if (user == null)
		{
			return TypedResults.NotFound();
		}

		return TypedResults.Ok(UserDTO.Mappings.FromEntity(user));
	}

	/// <summary>
	/// Get a user by username
	/// </summary>
	[HttpGet("by-username/{username}")]
	[EndpointName("get-user-by-username")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<Results<Ok<UserDTO>, NotFound>> GetUserByUsername(string username)
	{
		var user = await userService.GetUserByName(username);
		if (user == null)
		{
			return TypedResults.NotFound();
		}

		return TypedResults.Ok(UserDTO.Mappings.FromEntity(user));
	}

	/// <summary>
	/// Create a new user
	/// </summary>
	[HttpPost]
	[EndpointName("create-user")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDTO))]
	public async Task<Created<UserDTO>> CreateUser([FromBody] CreateUserDTO createDto)
	{
		var user = await userService.CreateUser(createDto.Username, createDto.Roles);
		var dto = UserDTO.Mappings.FromEntity(user);
		return TypedResults.Created($"/api/v1/user/{user.Id.Value}", dto);
	}

	/// <summary>
	/// Update an existing user
	/// </summary>
	[HttpPut("{id}")]
	[EndpointName("update-user")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<Results<Ok<UserDTO>, NotFound>> UpdateUser(long id, [FromBody] UpdateUserDTO updateDto)
	{
		var user = await userService.UpdateUser(
			UserId.From(id),
			updateDto.Username,
			updateDto.Roles
		);

		if (user == null)
		{
			return TypedResults.NotFound();
		}

		return TypedResults.Ok(UserDTO.Mappings.FromEntity(user));
	}

	/// <summary>
	/// Delete a user
	/// </summary>
	[HttpDelete("{id}")]
	[EndpointName("delete-user")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<Results<NoContent, NotFound>> DeleteUser(long id)
	{
		var success = await userService.DeleteUser(UserId.From(id));
		if (!success)
		{
			return TypedResults.NotFound();
		}

		return TypedResults.NoContent();
	}
}
