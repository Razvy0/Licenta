using System.ComponentModel.DataAnnotations;

namespace SkillSync.Core.DTOs.Swaps;

public class PickTimeDto
{
    [Required]
    public DateTime ScheduledDate { get; set; }
}
