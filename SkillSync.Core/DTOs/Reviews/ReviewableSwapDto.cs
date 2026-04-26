namespace SkillSync.Core.DTOs.Reviews;

public class ReviewableSwapDto
{
    public int SwapId { get; set; }
    public string OfferedSkillTitle { get; set; } = string.Empty;
    public string RequestedSkillTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
}
