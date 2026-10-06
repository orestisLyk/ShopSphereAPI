using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Service;
using System.Security.Claims;
using ShopSphere.Enums;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("{orderId}/create-intent")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePaymentIntent(int orderId)
        {
            var currentUserId = GetCurrentUserId();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == RoleName.Admin.ToString();
            var intent = await paymentService.CreatePaymentIntentAsync(orderId);
            return Ok(intent);
        }

        [HttpPost("{orderId}/cancel")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CancelPayment(int orderId)
        {
            await paymentService.CancelPaymentIntentAsync(orderId);
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
