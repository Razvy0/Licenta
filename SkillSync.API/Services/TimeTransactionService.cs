using SkillSync.Core.DTOs.TimeTransactions;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services
{
    public class TimeTransactionService : ITimeTransactionService
    {
        private readonly ITimeTransactionRepository _repo;

        public TimeTransactionService(ITimeTransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TimeTransactionDto>> GetUserTransactionsAsync(string userId)
        {
            var transactions = await _repo.GetByUserIdAsync(userId);
            return transactions.Select(t => new TimeTransactionDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Amount = t.Amount,
                TransactionType = t.TransactionType,
                SwapRequestId = t.SwapRequestId,
                CreatedAt = t.CreatedAt
            });
        }
    }
}
