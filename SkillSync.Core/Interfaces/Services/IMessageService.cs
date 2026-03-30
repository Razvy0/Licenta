using SkillSync.Core.DTOs.Messages;

namespace SkillSync.Core.Interfaces.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetConversationAsync(string userId, string otherUserId);
    Task<MessageDto> SendMessageAsync(string senderId, SendMessageDto dto);
    Task MarkAsReadAsync(string senderId, string receiverId);
}
