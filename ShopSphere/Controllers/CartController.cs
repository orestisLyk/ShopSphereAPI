using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Service;
using System.Security.Claims;
using ShopSphere.Enums;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart(int userId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            var cart = await cartService.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItem(int userId, [FromBody] CartItemCreateDTO dto)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            var cart = await cartService.AddItemToCartAsync(userId, dto);
            return Ok(cart);
        }

        [HttpPut("{userId}/items")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItem(int userId, [FromBody] CartItemUpdateDTO dto)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            var cart = await cartService.UpdateCartItemQuantityAsync(userId, dto);
            return Ok(cart);
        }

        [HttpDelete("{userId}/items/{productId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveItem(int userId, int productId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            await cartService.RemoveItemAsync(userId, productId);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            await cartService.ClearCartAsync(userId);
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var id))
            {
                throw new UnauthorizedAccessException("Invalid user token.");
            }
            return id;
        }
    }
}
