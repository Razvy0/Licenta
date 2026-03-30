using SkillSync.Core.DTOs.Users;

namespace SkillSync.Core.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(string userId);
    Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
}
