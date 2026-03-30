using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetConversationAsync(string userId1, string userId2);
    Task<IEnumerable<Message>> GetUnreadMessagesAsync(string userId);
    Task MarkAsReadAsync(string senderId, string receiverId);
}
