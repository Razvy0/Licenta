using Microsoft.EntityFrameworkCore;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Message>> GetConversationAsync(string userId1, string userId2)
        => await _dbSet
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2)
                     || (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(string userId)
        => await _dbSet
            .Include(m => m.Sender)
            .Where(m => m.ReceiverId == userId && !m.IsRead)
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();

    public async Task MarkAsReadAsync(string senderId, string receiverId)
    {
        var unread = await _dbSet
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId && !m.IsRead)
            .ToListAsync();

        foreach (var msg in unread)
            msg.IsRead = true;

        await _context.SaveChangesAsync();
    }
}
