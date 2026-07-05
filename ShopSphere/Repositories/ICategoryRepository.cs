using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category?> GetCategoryByName(string name);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CategoryNameExistsAsync(string name);
    }
}
