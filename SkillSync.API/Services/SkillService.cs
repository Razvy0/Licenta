using SkillSync.Core.DTOs.Skills;
using SkillSync.Core.Entities;
using SkillSync.Core.Exceptions;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _skillRepo;
    private readonly ICategoryRepository _categoryRepo;

    public SkillService(ISkillRepository skillRepo, ICategoryRepository categoryRepo)
    {
        _skillRepo = skillRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<IEnumerable<SkillDto>> GetSkillsAsync(SkillQueryParams queryParams)
    {
        var skills = await _skillRepo.GetSkillsWithDetailsAsync(
            queryParams.Category, queryParams.Search, queryParams.IsOffering,
            queryParams.Page, queryParams.PageSize);

        return skills.Select(MapToDto);
    }

    public async Task<SkillDto> GetSkillByIdAsync(int id)
    {
        var skill = await _skillRepo.GetSkillWithDetailsAsync(id)
            ?? throw new NotFoundException("Skill", id);
        return MapToDto(skill);
    }

    public async Task<SkillDto> CreateSkillAsync(string userId, CreateSkillDto dto)
    {
        if (!await _categoryRepo.ExistsAsync(dto.CategoryId))
            throw new NotFoundException("Category", dto.CategoryId);

        var skill = new Skill
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            ProficiencyLevel = dto.ProficiencyLevel,
            IsOffering = dto.IsOffering
        };

        await _skillRepo.AddAsync(skill);
        return await GetSkillByIdAsync(skill.Id);
    }

    public async Task DeleteSkillAsync(string userId, int id)
    {
        var skill = await _skillRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Skill", id);

        if (skill.UserId != userId)
            throw new UnauthorizedException("You can only delete your own skills.");

        await _skillRepo.DeleteAsync(skill);
    }

    private static SkillDto MapToDto(Skill s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Description = s.Description,
        ProficiencyLevel = s.ProficiencyLevel,
        IsOffering = s.IsOffering,
        CategoryName = s.Category?.Name ?? "",
        CategoryId = s.CategoryId,
        UserId = s.UserId,
        UserFullName = s.User?.FullName ?? "",
        CreatedAt = s.CreatedAt
    };
}
