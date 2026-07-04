using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopSphereContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await context.Categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryByName(string name)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
