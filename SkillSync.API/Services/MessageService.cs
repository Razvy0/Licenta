using SkillSync.Core.DTOs.Messages;
using SkillSync.Core.Entities;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;

    public MessageService(IMessageRepository messageRepo, IUserRepository userRepo)
    {
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    public async Task<IEnumerable<MessageDto>> GetConversationAsync(string userId, string otherUserId)
    {
        var messages = await _messageRepo.GetConversationAsync(userId, otherUserId);
        return messages.Select(MapToDto);
    }

    public async Task<MessageDto> SendMessageAsync(string senderId, SendMessageDto dto)
    {
        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            Content = dto.Content
        };

        await _messageRepo.AddAsync(message);

        var sender = await _userRepo.GetByIdAsync(senderId);
        var receiver = await _userRepo.GetByIdAsync(dto.ReceiverId);

        return new MessageDto
        {
            Id = message.Id,
            SenderId = senderId,
            SenderName = sender?.FullName ?? "",
            ReceiverId = dto.ReceiverId,
            ReceiverName = receiver?.FullName ?? "",
            Content = message.Content,
            Timestamp = message.Timestamp,
            IsRead = message.IsRead
        };
    }

    public async Task MarkAsReadAsync(string senderId, string receiverId)
        => await _messageRepo.MarkAsReadAsync(senderId, receiverId);

    private static MessageDto MapToDto(Message m) => new()
    {
        Id = m.Id,
        SenderId = m.SenderId,
        SenderName = m.Sender?.FullName ?? "",
        ReceiverId = m.ReceiverId,
        ReceiverName = m.Receiver?.FullName ?? "",
        Content = m.Content,
        Timestamp = m.Timestamp,
        IsRead = m.IsRead
    };
}
