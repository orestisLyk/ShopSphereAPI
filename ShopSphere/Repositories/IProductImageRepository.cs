using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface IProductImageRepository : IBaseRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage?> GetImageByIdAsync(int id);
        Task<bool> ImageExistsForProductAsync(int productId);
    }
}
