using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Core.DTOs.Users;
using SkillSync.Core.Interfaces.Services;
using System.Security.Claims;

namespace SkillSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(string id)
    {
        var profile = await _userService.GetProfileAsync(id);
        return Ok(profile);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var profile = await _userService.UpdateProfileAsync(userId, dto);
        return Ok(profile);
    }
}
