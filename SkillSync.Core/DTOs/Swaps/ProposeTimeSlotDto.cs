using System.ComponentModel.DataAnnotations;

namespace SkillSync.Core.DTOs.Swaps;

public class ProposeTimeSlotDto
{
    [Required]
    public DateTime TimeSlotStart { get; set; }

    [Required]
    public DateTime TimeSlotEnd { get; set; }
}
