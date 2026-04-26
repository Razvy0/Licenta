using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Core.DTOs.Analytics;
using SkillSync.Core.Interfaces.Services;
using System.Security.Claims;

namespace SkillSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardAnalyticsDto>> GetDashboardAnalytics()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var analytics = await _analyticsService.GetDashboardAnalyticsAsync(userId);
        return Ok(analytics);
    }
}
