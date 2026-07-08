using AutoMapper;
using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ProductService> logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ProductDetailsDTO?> GetProductDetailsByIdAsync(int id)
        {
            var product = await unitOfWork.ProductRepository.GetProductDetailsAsync(id);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with ID {id} not found.");
            }
            var productDetailsDTO = mapper.Map<ProductDetailsDTO>(product);
            logger.LogInformation("Retrieved product details for product ID {ProductId}", id);
            return productDetailsDTO;
        }

        public async Task<ProductDetailsDTO?> GetProductDetailsBySkuAsync(string sku)
        {
            var product = await unitOfWork.ProductRepository.GetProductDetailsBySkuAsync(sku);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with SKU {sku} not found.");
            }
            var productDetailsDTO = mapper.Map<ProductDetailsDTO>(product);
            logger.LogInformation("Retrieved product details for product SKU {ProductSku}", sku);
            return productDetailsDTO;
        }

        public async Task<PaginatedResult<ProductMinimalDTO>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var paginatedProducts = await unitOfWork.ProductRepository.GetProductsAsync(pageNumber, pageSize);
            var productMinimalDTOs = mapper.Map<List<ProductMinimalDTO>>(paginatedProducts.Items);
            logger.LogInformation("Retrieved products for page {PageNumber} with page size {PageSize}", pageNumber, pageSize);
            return new PaginatedResult<ProductMinimalDTO>(productMinimalDTOs, paginatedProducts.TotalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<ProductMinimalDTO>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            var paginatedProducts = await unitOfWork.ProductRepository.GetProductsByCategoryAsync(categoryId, pageNumber, pageSize);
            var productMinimalDTOs = mapper.Map<List<ProductMinimalDTO>>(paginatedProducts.Items);
            logger.LogInformation("Retrieved products for category ID {CategoryId} on page {PageNumber} with page size {PageSize}", categoryId, pageNumber, pageSize);
            return new PaginatedResult<ProductMinimalDTO>(productMinimalDTOs, paginatedProducts.TotalRecords, pageNumber, pageSize);
        }

        public async Task<ProductDetailsDTO?> CreateProductAsync(ProductCreateDTO dto)
        {
            var product = mapper.Map<Product>(dto);

            string sku = "";
            do
            {
                sku = GenerateSku(dto.Name);
            } while (await unitOfWork.ProductRepository.SkuExistsAsync(sku));

            product.Sku = sku;

            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            var productDetailsDTO = mapper.Map<ProductDetailsDTO>(product);
            logger.LogInformation("Created new product with ID {ProductId}", product.Id);
            return productDetailsDTO;
        }

        public async Task<ProductDetailsDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto)
        {
            var product = await unitOfWork.ProductRepository.GetAsync(id);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with ID {id} not found.");
            }
            mapper.Map(dto, product);
            await unitOfWork.SaveChangesAsync();
            var productDetailsDTO = mapper.Map<ProductDetailsDTO>(product);
            logger.LogInformation("Updated product with ID {ProductId}", product.Id);
            return productDetailsDTO;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await unitOfWork.ProductRepository.GetAsync(id);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with ID {id} not found.");
            }
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Deleted product with ID {ProductId}", product.Id);
        }

        private string GenerateSku(string productName)
        {
            var random = new Random();
            var randomNumber = random.Next(1000, 9999);
            var sku = $"{productName.Substring(0, Math.Min(productName.Length, 3)).ToUpper()}{randomNumber}";
            return sku;
        }
    }
}
