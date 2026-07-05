namespace ShopSphere.Service
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(
            int houseId,
            IFormFile file
            );
    }
}
