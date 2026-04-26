using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<AppUser?> GetByIdAsync(string id)
        => await _context.Users.FindAsync(id);

    public async Task<AppUser?> GetByIdWithSkillsAsync(string id)
        => await _context.Users
            .Include(u => u.Skills).ThenInclude(s => s.Category)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task UpdateAsync(AppUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AppUser>> SearchUsersAsync(string? name, string? skill, int page, int pageSize)
    {
        var query = _context.Users
            .Include(u => u.Skills).ThenInclude(s => s.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(skill))
        {
            query = query.Where(u =>
                u.FullName.ToLower().Contains(name.ToLower())
                || u.Skills.Any(s =>
                    s.Title.ToLower().Contains(skill.ToLower())
                    || s.Category.Name.ToLower().Contains(skill.ToLower())));
        }
        else if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(u => u.FullName.ToLower().Contains(name.ToLower()));
        }
        else if (!string.IsNullOrWhiteSpace(skill))
        {
            query = query.Where(u => u.Skills.Any(s =>
                s.Title.ToLower().Contains(skill.ToLower())
                || s.Category.Name.ToLower().Contains(skill.ToLower())));
        }

        return await query
            .OrderBy(u => u.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
