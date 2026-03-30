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

    public AppUser Requester { get; set; } = null!;
    public AppUser Receiver { get; set; } = null!;
    public Skill OfferedSkill { get; set; } = null!;
    public Skill RequestedSkill { get; set; } = null!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
