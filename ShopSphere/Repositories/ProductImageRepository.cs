using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ShopSphereContext context) : base(context) { }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductImage?> GetImageByIdAsync(int id)
        {
            return await context.ProductImages
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task<bool> ImageExistsForProductAsync(int productId)
        {
            return await context.ProductImages
                .AnyAsync(pi => pi.ProductId == productId);
        }
    }
}
