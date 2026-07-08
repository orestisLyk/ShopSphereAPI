using ShopSphere.Core;
using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadOnlyDTO>> GetAllCategoriesAsync();
        Task<CategoryReadOnlyDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryReadOnlyDTO?> CreateCategoryAsync(CategoryCreateDTO dto);
        Task<CategoryReadOnlyDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto);
        Task DeleteCategoryAsync(int id);

    }
}
