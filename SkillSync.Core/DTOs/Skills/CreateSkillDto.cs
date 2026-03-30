using System.ComponentModel.DataAnnotations;
using SkillSync.Core.Enums;

namespace SkillSync.Core.DTOs.Skills;

public class CreateSkillDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Beginner;
    public bool IsOffering { get; set; } = true;
}
