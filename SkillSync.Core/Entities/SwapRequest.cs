using SkillSync.Core.Enums;

namespace SkillSync.Core.Entities;

public class SwapRequest
{
    public int Id { get; set; }
    public string RequesterId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public int OfferedSkillId { get; set; }
    public int RequestedSkillId { get; set; }
    public SwapStatus Status { get; set; } = SwapStatus.Pending;
    public DateTime? ScheduledDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Time-slot scheduling: receiver proposes a window, requester picks a time
    public DateTime? TimeSlotStart { get; set; }
    public DateTime? TimeSlotEnd { get; set; }

    // Validation: both parties must confirm the swap was fulfilled
    public bool RequesterValidated { get; set; }
    public bool ReceiverValidated { get; set; }

    public AppUser Requester { get; set; } = null!;
    public AppUser Receiver { get; set; } = null!;
    public Skill OfferedSkill { get; set; } = null!;
    public Skill RequestedSkill { get; set; } = null!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();
}
