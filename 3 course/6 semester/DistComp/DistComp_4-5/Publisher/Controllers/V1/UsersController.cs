using Microsoft.AspNetCore.Mvc;
using Publisher.DTO.RequestDTO;
using Publisher.Services.Interfaces;

namespace Publisher.Controllers.V1;

[ApiController]
[Route("api/v1.0/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(long id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserRequestDTO user)
    {
        var updatedUser = await _userService.UpdateUserAsync(user);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}