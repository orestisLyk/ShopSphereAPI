using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartWithItemsByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<bool> CartExistsForUserAsync(int userId);
    }
}
