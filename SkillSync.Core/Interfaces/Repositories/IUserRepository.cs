using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<AppUser?> GetByIdAsync(string id);
    Task<AppUser?> GetByIdWithSkillsAsync(string id);
    Task UpdateAsync(AppUser user);
}
