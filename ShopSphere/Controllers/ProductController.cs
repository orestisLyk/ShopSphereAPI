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

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productCreateDTO">The product to create.</param>
        /// <returns>The created product.</returns>
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

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productUpdateDTO">The updated product information.</param>
        /// <returns>The updated product.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductUpdateDTO productUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = await productService.UpdateProductAsync(id, productUpdateDTO);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await productService.DeleteProductAsync(id);
            
            return NoContent();
        }


    }
}
