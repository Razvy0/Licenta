using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class TimeTransactionRepository : ITimeTransactionRepository
{
    private readonly AppDbContext _context;

    public TimeTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TimeTransaction?> GetByIdAsync(Guid id)
    {
        return await _context.TimeTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<TimeTransaction>> GetByUserIdAsync(string userId)
    {
        return await _context.TimeTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(TimeTransaction transaction)
    {
        await _context.TimeTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimeTransaction transaction)
    {
        _context.TimeTransactions.Update(transaction);
        await _context.SaveChangesAsync();
    }
}
