using System.ComponentModel.DataAnnotations;
using SkillSync.Core.Enums;

namespace SkillSync.Core.DTOs.Swaps;

public class UpdateSwapStatusDto
{
    [Required]
    public SwapStatus Status { get; set; }
}
