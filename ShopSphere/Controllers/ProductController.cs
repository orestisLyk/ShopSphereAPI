using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Service;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Retrieves all products with pagination.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="size">The number of items per page.</param>
        /// <returns>A paginated list of products.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<ProductMinimalDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int page, [FromQuery] int size)
        {
            var products = await productService.GetAllProductsAsync(page, size);
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The details of the product.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await productService.GetProductDetailsByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Retrieves a product by its SKU
        /// </summary>
        /// <param name="sku">The SKU of the product</param>
        /// <returns>The details of the product</returns>
        [HttpGet("sku/{sku}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductBySkuAsync(string sku)
        {
            var product = await productService.GetProductDetailsBySkuAsync(sku);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Retrieves products by their category ID with pagination.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="page">The page number.</param>
        /// <param name="size">The number of items per page.</param>
        /// <returns>A paginated list of products in the specified category.</returns>
        [HttpGet("by-category/{categoryId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<ProductMinimalDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByCategory(int categoryId, [FromQuery] int page, [FromQuery] int size)
        {
            var products = await productService.GetProductsByCategoryAsync(categoryId, page, size);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDTO productCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await productService.CreateProductAsync(productCreateDTO);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdProduct.Id }, createdProduct);
        }
    }
}
