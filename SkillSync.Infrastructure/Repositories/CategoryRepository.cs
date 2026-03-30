using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<Category?> GetByNameAsync(string name)
        => await _dbSet.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
}
