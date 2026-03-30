using SkillSync.Core.DTOs.Categories;

namespace SkillSync.Core.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
}
