using System.ComponentModel.DataAnnotations;

namespace SkillSync.Core.DTOs.Swaps;

public class CreateSwapDto
{
    [Required]
    public int OfferedSkillId { get; set; }

    [Required]
    public int RequestedSkillId { get; set; }

    public DateTime? ScheduledDate { get; set; }
}
