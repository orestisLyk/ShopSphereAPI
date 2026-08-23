using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Enums;
using ShopSphere.Service;
using System.Security.Claims;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<CategoryReadOnlyDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category with the specified ID.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CategoryReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryReadOnlyDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString())
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await categoryService.CreateCategoryAsync(categoryCreateDTO);
            return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = createdCategory?.Id }, createdCategory);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryUpdateDTO">The updated category data.</param>
        /// <returns>The updated category.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryReadOnlyDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString())
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCategory = await categoryService.UpdateCategoryAsync(id, categoryUpdateDTO);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString())
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            await categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
