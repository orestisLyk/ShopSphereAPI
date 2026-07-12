using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface ICartService
    {
        Task<CartReadOnlyDTO?> GetCartByUserIdAsync(int userId);
        Task<CartReadOnlyDTO?> AddItemToCartAsync(int userId, CartItemCreateDTO dto);
        Task<CartReadOnlyDTO?> UpdateCartItemQuantityAsync(int userId, CartItemUpdateDTO dto);
        Task RemoveItemAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
