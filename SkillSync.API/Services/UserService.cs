using SkillSync.Core.DTOs.Users;
using SkillSync.Core.Exceptions;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo) => _userRepo = userRepo;

    public async Task<UserProfileDto> GetProfileAsync(string userId)
    {
        var user = await _userRepo.GetByIdWithSkillsAsync(userId)
            ?? throw new NotFoundException("User", userId);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Bio = user.Bio,
            TimeBalance = user.TimeBalance,
            Rating = user.Rating,
            Skills = user.Skills.Select(s => new SkillSummaryDto
            {
                Id = s.Id,
                Title = s.Title,
                IsOffering = s.IsOffering,
                CategoryName = s.Category?.Name ?? ""
            }).ToList()
        };
    }

    public async Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new NotFoundException("User", userId);

        if (dto.FullName != null) user.FullName = dto.FullName;
        if (dto.Bio != null) user.Bio = dto.Bio;

        await _userRepo.UpdateAsync(user);
        return await GetProfileAsync(userId);
    }

    public async Task<IEnumerable<UserSearchResultDto>> SearchUsersAsync(UserSearchParams searchParams)
    {
        var users = await _userRepo.SearchUsersAsync(
            searchParams.Name, searchParams.Skill,
            searchParams.Page, searchParams.PageSize);

        return users.Select(u => new UserSearchResultDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Bio = u.Bio,
            Rating = u.Rating,
            Skills = u.Skills.Select(s => new SkillSummaryDto
            {
                Id = s.Id,
                Title = s.Title,
                IsOffering = s.IsOffering,
                CategoryName = s.Category?.Name ?? ""
            }).ToList()
        });
    }
}
