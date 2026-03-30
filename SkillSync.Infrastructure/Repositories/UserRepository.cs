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
}
