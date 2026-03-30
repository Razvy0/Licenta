using System.ComponentModel.DataAnnotations;

namespace SkillSync.Core.DTOs.Messages;

public class SendMessageDto
{
    [Required]
    public string ReceiverId { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
}
