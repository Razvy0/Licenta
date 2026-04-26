using SkillSync.Core.DTOs.Analytics;

namespace SkillSync.Core.Interfaces.Services;

public interface IAnalyticsService
{
    Task<DashboardAnalyticsDto> GetDashboardAnalyticsAsync(string userId);
}
