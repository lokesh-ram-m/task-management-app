using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.User;
using TaskManagementApi.Models;
using TaskManagementApi.Services.Users;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<UserResponse> users = this._userService.GetAll();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        UserResponse user = this._userService.GetById(id);

        return Ok(user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UserResponse request)
    {
        this._userService.Update(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Deactivate(int id)
    {
        this._userService.Deactivate(id);

        return NoContent();
    }
}
