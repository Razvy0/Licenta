using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface IDisputeRepository : IRepository<Dispute>
{
    Task<IEnumerable<Dispute>> GetDisputesByUserIdAsync(string userId);
    Task<IEnumerable<Dispute>> GetDisputesBySwapRequestIdAsync(int swapRequestId);
}