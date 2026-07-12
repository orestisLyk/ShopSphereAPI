using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ShopSphereContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetCartWithItemsByUserIdAsync(int userId)
        {
            return await context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId && !ci.Product.IsDeleted);
        }

        public async Task<bool> CartExistsForUserAsync(int userId)
        {
            return await context.Carts.AnyAsync(c => c.UserId == userId);
        }
    }
}
