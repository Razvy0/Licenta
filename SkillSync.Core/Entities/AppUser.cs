using Microsoft.AspNetCore.Identity;

namespace SkillSync.Core.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public int TimeBalance { get; set; } = 5;
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
    public ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
}
