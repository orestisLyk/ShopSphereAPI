using Microsoft.EntityFrameworkCore;
using ShopSphere.Core;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopSphereContext context) : base(context)
        {
        }

        public async Task<Product?> GetProductDetailsBySkuAsync(string sku)
        {
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Sku == sku);
        }

        public async Task<Product?> GetProductDetailsAsync(int id)
        {
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PaginatedResult<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var totalCount = await context.Products.CountAsync();
            var products = await context.Products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedResult<Product>(products, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<Product>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var totalCount = await context.Products.CountAsync(p => p.CategoryId == categoryId);
            var products = await context.Products
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedResult<Product>(products, totalCount, pageNumber, pageSize);
        }

        public async Task<bool> SkuExistsAsync(string sku)
        {
            return await context.Products.AnyAsync(p => p.Sku == sku);
        }
    }
}
