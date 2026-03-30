using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class SwapRepository : Repository<SwapRequest>, ISwapRepository
{
    public SwapRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<SwapRequest>> GetSwapsByUserIdAsync(string userId)
        => await _dbSet
            .Include(s => s.Requester)
            .Include(s => s.Receiver)
            .Include(s => s.OfferedSkill)
            .Include(s => s.RequestedSkill)
            .Where(s => s.RequesterId == userId || s.ReceiverId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

    public async Task<SwapRequest?> GetSwapWithDetailsAsync(int id)
        => await _dbSet
            .Include(s => s.Requester)
            .Include(s => s.Receiver)
            .Include(s => s.OfferedSkill)
            .Include(s => s.RequestedSkill)
            .FirstOrDefaultAsync(s => s.Id == id);
}
