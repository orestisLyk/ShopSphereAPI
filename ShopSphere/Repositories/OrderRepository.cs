using Microsoft.EntityFrameworkCore;
using ShopSphere.Core;
using ShopSphere.Data;
using ShopSphere.Enums;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopSphereContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithItemsAsync(int id)
        {
            return await context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<PaginatedResult<Order>> GetOrdersByUserAsync(int userId, int pageNumber, int pageSize)
        {
            var orders = context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalOrders = await context.Orders.CountAsync(o => o.UserId == userId);

            return new PaginatedResult<Order>(orders, totalOrders, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<Order>> GetOrdersByStatusAsync(OrderStatus status, int pageNumber, int pageSize)
        {
            var orders = context.Orders
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var totalOrders = await context.Orders.CountAsync(o => o.Status == status);
            return new PaginatedResult<Order>(orders, totalOrders, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<Order>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var orders = context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var totalOrders = await context.Orders.CountAsync();
            return new PaginatedResult<Order>(orders, totalOrders, pageNumber, pageSize);
        }
    }
}
