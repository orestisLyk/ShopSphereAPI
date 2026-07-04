using ShopSphere.Core;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product?> GetProductBySkuAsync(string sku);
        Task<Product?> GetProductDetailsAsync(int id);
        Task<PaginatedResult<Product>> GetProductsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<Product>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    }
}
