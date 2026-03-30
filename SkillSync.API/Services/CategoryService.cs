using SkillSync.Core.DTOs.Categories;
using SkillSync.Core.Entities;
using SkillSync.Core.Exceptions;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(ICategoryRepository categoryRepo) => _categoryRepo = categoryRepo;

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description });
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var existing = await _categoryRepo.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new BadRequestException($"Category '{dto.Name}' already exists.");

        var category = new Category { Name = dto.Name, Description = dto.Description };
        await _categoryRepo.AddAsync(category);

        return new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description };
    }
}
