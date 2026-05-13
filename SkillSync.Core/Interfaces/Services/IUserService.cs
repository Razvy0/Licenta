using SkillSync.Core.DTOs.Users;

using SkillSync.Core.DTOs.Common;

namespace SkillSync.Core.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(string userId);
    Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
    Task<PagedResult<UserSearchResultDto>> SearchUsersAsync(UserSearchParams searchParams);
}
