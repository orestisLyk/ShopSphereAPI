using ShopSphere.Core;
using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface IProductService
    {
        Task<ProductDetailsDTO?> GetProductDetailsByIdAsync(int id);
        Task<ProductDetailsDTO?> GetProductDetailsBySkuAsync(string sku);
        Task<PaginatedResult<ProductMinimalDTO>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<ProductMinimalDTO>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);

        Task<ProductDetailsDTO?> CreateProductAsync(ProductCreateDTO dto);
        Task<ProductDetailsDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto);
        Task DeleteProductAsync(int id);
    }
}
