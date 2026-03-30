using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Core.DTOs.Reviews;
using SkillSync.Core.Interfaces.Services;
using System.Security.Claims;

namespace SkillSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService) => _reviewService = reviewService;

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
    {
        var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var review = await _reviewService.CreateReviewAsync(userId, dto);
        return CreatedAtAction(nameof(GetByUser), new { userId = review.RevieweeId }, review);
    }
}
