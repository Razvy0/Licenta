using SkillSync.Core.Enums;

namespace SkillSync.Core.DTOs.Swaps;

public class SwapDto
{
    public int Id { get; set; }
    public string RequesterId { get; set; } = string.Empty;
    public string RequesterName { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string OfferedSkillTitle { get; set; } = string.Empty;
    public string RequestedSkillTitle { get; set; } = string.Empty;
    public SwapStatus Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? TimeSlotStart { get; set; }
    public DateTime? TimeSlotEnd { get; set; }
    public bool RequesterValidated { get; set; }
    public bool ReceiverValidated { get; set; }
    public DateTime CreatedAt { get; set; }
}
