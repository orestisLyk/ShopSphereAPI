using ShopSphere.Core;
using ShopSphere.Enums;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int id);
        Task<PaginatedResult<Order>> GetOrdersByUserAsync(int userId, int pageNumber, int pageSize);
        Task<PaginatedResult<Order>> GetOrdersByStatusAsync(OrderStatus status, int pageNumber, int pageSize);
        Task<PaginatedResult<Order>> GetAllOrdersAsync(int pageNumber, int pageSize);
    }
}
