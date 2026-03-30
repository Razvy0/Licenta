using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}
