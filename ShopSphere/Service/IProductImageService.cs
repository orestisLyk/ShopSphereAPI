using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface IProductImageService
    {
        Task AddImageAsync(int productId, ProductImageCreateDTO image);
        Task DeleteImageAsync(int imageId);
    }
}
