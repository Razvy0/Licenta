using SkillSync.Core.Entities;

namespace SkillSync.Core.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetConversationAsync(string userId1, string userId2);
    Task<IEnumerable<Message>> GetUnreadMessagesAsync(string userId);
    Task MarkAsReadAsync(string senderId, string receiverId);
    Task<IEnumerable<(AppUser Partner, Message LastMessage, int UnreadCount)>> GetConversationPartnersAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
}
