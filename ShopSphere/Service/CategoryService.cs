using AutoMapper;
using ShopSphere.DTO;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CategoryService> logger;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<CategoryReadOnlyDTO>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            return mapper.Map<List<CategoryReadOnlyDTO>>(categories);
        }

        public async Task<CategoryReadOnlyDTO?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetAsync(categoryId);
            if (category == null)
            {
                logger.LogWarning($"Category with ID {categoryId} not found.");
                throw new EntityNotFoundException($"Category with ID {categoryId} not found.");
            }
            return mapper.Map<CategoryReadOnlyDTO>(category);
        }

        public async Task<CategoryReadOnlyDTO?> CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO)
        {
            bool nameExists = await unitOfWork.CategoryRepository.CategoryNameExistsAsync(categoryCreateDTO.Name);
            if (nameExists)
            {
                throw new EntityAlreadyExistsException($"Category with name {categoryCreateDTO.Name} already exists.");
            }
            var category = mapper.Map<Category>(categoryCreateDTO);
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Category with name {categoryCreateDTO.Name} created successfully.");
            return mapper.Map<CategoryReadOnlyDTO>(category);
        }

        public async Task<CategoryReadOnlyDTO?> UpdateCategoryAsync(int categoryId, CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = await unitOfWork.CategoryRepository.GetAsync(categoryId);
            if (category == null)
            {
                logger.LogWarning($"Category with ID {categoryId} not found.");
                throw new EntityNotFoundException($"Category with ID {categoryId} not found.");
            }
            if(category.Name != categoryUpdateDTO.Name)
            {
                bool nameExists = await unitOfWork.CategoryRepository.CategoryNameExistsAsync(categoryUpdateDTO.Name);
                if (nameExists)
                {
                    throw new EntityAlreadyExistsException($"Category with name {categoryUpdateDTO.Name} already exists.");
                }
            }
            mapper.Map(categoryUpdateDTO, category);
            
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Category with ID {categoryId} updated successfully.");
            return mapper.Map<CategoryReadOnlyDTO>(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetAsync(categoryId);
            if (category == null)
            {
                throw new EntityNotFoundException($"Category with ID {categoryId} not found.");
            }
            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Category with ID {categoryId} deleted successfully.");
        }
    }
}
