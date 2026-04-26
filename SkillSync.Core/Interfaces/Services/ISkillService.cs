using SkillSync.Core.DTOs.Skills;

namespace SkillSync.Core.Interfaces.Services;

public interface ISkillService
{
    Task<IEnumerable<SkillDto>> GetSkillsAsync(SkillQueryParams queryParams);
    Task<SkillDto> GetSkillByIdAsync(int id);
    Task<SkillDto> CreateSkillAsync(string userId, CreateSkillDto dto);
    Task DeleteSkillAsync(string userId, int id);
    Task<IEnumerable<SkillDto>> GetUserSkillsAsync(string userId);
}
