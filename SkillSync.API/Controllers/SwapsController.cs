using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Core.DTOs.Swaps;
using SkillSync.Core.Interfaces.Services;
using System.Security.Claims;

namespace SkillSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SwapsController : ControllerBase
{
    private readonly ISwapService _swapService;

    public SwapsController(ISwapService swapService) => _swapService = swapService;

    [HttpGet]
    public async Task<IActionResult> GetSwaps()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var swaps = await _swapService.GetUserSwapsAsync(userId);
        return Ok(swaps);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSwap([FromBody] CreateSwapDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var swap = await _swapService.CreateSwapAsync(userId, dto);
        return CreatedAtAction(nameof(GetSwaps), swap);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateSwapStatusDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var swap = await _swapService.UpdateSwapStatusAsync(userId, id, dto);
        return Ok(swap);
    }
}
