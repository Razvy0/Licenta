using SkillSync.Core.DTOs.Disputes;

namespace SkillSync.Core.Interfaces.Services;

public interface IDisputeService
{
    Task<DisputeDto> CreateDisputeAsync(string currentUserId, CreateDisputeDto dto);
    Task<IEnumerable<DisputeDto>> GetUserDisputesAsync(string userId);
}