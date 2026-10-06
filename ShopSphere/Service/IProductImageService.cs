using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface IProductImageService
    {
        Task<ProductImageReadDTO> AddImageAsync(int productId, ProductImageCreateDTO image);
        Task DeleteImageAsync(int imageId);
        Task<IEnumerable<ProductImageReadDTO>> GetImagesByProductIdAsync(int productId);
        Task<ProductImageReadDTO?> GetImageByIdAsync(int imageId);
    }
}
