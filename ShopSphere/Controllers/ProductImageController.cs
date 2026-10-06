using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Service;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/product-images")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            this.productImageService = productImageService;
        }

        /// <summary>
        /// Uploads an image for a product. Admin only.
        /// </summary>
        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImageAsync(int id, [FromForm] ProductImageCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await productImageService.AddImageAsync(id, dto);
            return CreatedAtAction(nameof(GetImageByIdAsync), new { imageId = created.Id }, created);
        }

        /// <summary>
        /// Soft-deletes a product image. Admin only.
        /// </summary>
        [HttpDelete("{imageId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImageAsync(int imageId)
        {
            await productImageService.DeleteImageAsync(imageId);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all images for a product.
        /// </summary>
        [HttpGet("by-product/{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            var images = await productImageService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }

        /// <summary>
        /// Retrieves a single image by id.
        /// </summary>
        [HttpGet("image/{imageId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageByIdAsync(int imageId)
        {
            var image = await productImageService.GetImageByIdAsync(imageId);
            if (image == null) return NotFound();
            return Ok(image);
        }
    }
}
