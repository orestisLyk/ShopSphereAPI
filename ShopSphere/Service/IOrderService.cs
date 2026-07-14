using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Enums;

namespace ShopSphere.Service
{
    public interface IOrderService
    {
        Task<OrderReadOnlyDTO?> CheckoutAsync(int userId);
        Task<PaginatedResult<OrderReadOnlyDTO>> GetOrdersByUserAsync(int userId,int currentUserId,bool isAdmin, int pageNumber, int pageSize);
        Task<OrderReadOnlyDTO?> GetOrderByIdAsync(int orderId, int currentUserId, bool isAdmin);
        Task CancelOrderAsync(int orderId, int currentUserId);
        Task<OrderReadOnlyDTO?> UpdateOrderStatus(int orderId, OrderStatus status, bool isAdmin);
    }
}
