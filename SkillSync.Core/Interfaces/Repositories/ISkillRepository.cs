using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface ISkillRepository : IRepository<Skill>
{
    Task<IEnumerable<Skill>> GetSkillsWithDetailsAsync(string? category, string? search, bool? isOffering, int page, int pageSize);
    Task<IEnumerable<Skill>> GetSkillsByUserIdAsync(string userId);
    Task<Skill?> GetSkillWithDetailsAsync(int id);
}
