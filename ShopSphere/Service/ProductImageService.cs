using AutoMapper;
using ShopSphere.DTO;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ProductImageService> logger;
        private readonly IMapper mapper;
        private readonly IImageStorageService imageStorageService;

        public ProductImageService(IUnitOfWork unitOfWork, ILogger<ProductImageService> logger, IMapper mapper, IImageStorageService imageStorageService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
            this.imageStorageService = imageStorageService;
        }

        public async Task AddImageAsync(int productId, ProductImageCreateDTO image)
        {
            var product = await unitOfWork.ProductRepository.GetProductDetailsAsync(productId);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with ID {productId} not found.");
            }

            ProductImage imageEntity = new ProductImage();
            imageEntity.ImageUrl = await imageStorageService.UploadImageAsync(productId, image.ImageFile);
            imageEntity.ProductId = productId;

            await unitOfWork.ProductImageRepository.AddAsync(imageEntity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var imageEntity = await unitOfWork.ProductImageRepository.GetAsync(imageId);
            if (imageEntity == null)
            {
                throw new EntityNotFoundException($"Image with ID {imageId} not found.");
            }

            imageEntity.IsDeleted = true;
            imageEntity.DeletedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
        }
    }
}
