using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Service;
using System.Security.Claims;
using ShopSphere.Enums;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("checkout/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Checkout(int userId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != RoleName.Admin.ToString() && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
            }
            var order = await orderService.CheckoutAsync(userId);
            if (order == null) return NotFound();
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdersByUser(int userId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == RoleName.Admin.ToString();
            if (!isAdmin && currentUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            var orders = await orderService.GetOrdersByUserAsync(userId, currentUserId, isAdmin, page, size);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == RoleName.Admin.ToString();
            var order = await orderService.GetOrderByIdAsync(orderId, currentUserId, isAdmin);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("{orderId}/cancel")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var currentUserId = GetCurrentUserId();
            await orderService.CancelOrderAsync(orderId, currentUserId);
            return NoContent();
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] ShopSphere.Enums.OrderStatus status)
        {
            var updated = await orderService.UpdateOrderStatus(orderId, status, true);
            if (updated == null) return NotFound();
            return Ok(updated);
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
