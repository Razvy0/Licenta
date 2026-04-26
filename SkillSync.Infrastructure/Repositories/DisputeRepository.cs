using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class DisputeRepository : Repository<Dispute>, IDisputeRepository
{
    private readonly AppDbContext _context;

    public DisputeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dispute>> GetDisputesByUserIdAsync(string userId)
    {
         return await _context.Disputes
            .Include(d => d.Reporter)
            .Include(d => d.ReportedUser)
            .Where(d => d.ReporterId == userId || d.ReportedUserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dispute>> GetDisputesBySwapRequestIdAsync(int swapRequestId)
    {
        return await _context.Disputes
            .Include(d => d.Reporter)
            .Include(d => d.ReportedUser)
            .Where(d => d.SwapRequestId == swapRequestId)
            .ToListAsync();
    }
}