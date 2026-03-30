using SkillSync.Core.DTOs.Reviews;

namespace SkillSync.Core.Interfaces.Services;

public interface IReviewService
{
    Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(string userId);
    Task<ReviewDto> CreateReviewAsync(string reviewerId, CreateReviewDto dto);
}
