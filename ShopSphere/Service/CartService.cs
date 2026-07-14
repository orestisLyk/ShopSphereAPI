using AutoMapper;
using ShopSphere.DTO;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CartService> logger;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CartService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<CartReadOnlyDTO?> AddItemToCartAsync(int userId, CartItemCreateDTO dto)
        {
            if(dto.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantity must be greater than zero.");
            }

            var user = await unitOfWork.UserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {userId} not found.");
            }

            var product = await unitOfWork.ProductRepository.GetAsync(dto.ProductId);
            if (product == null || product.IsDeleted)
            {
                throw new EntityNotFoundException($"Product with ID {dto.ProductId} not found.");
            }

            var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await unitOfWork.CartRepository.AddAsync(cart);
                await unitOfWork.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            var resultingQuantity = (cartItem?.Quantity ?? 0) + dto.Quantity;
            if(resultingQuantity > product.StockQuantity)
            {
                throw new InsufficientStockException($"Cannot add {dto.Quantity} of product '{product.Name}' to cart. Only {product.StockQuantity - (cartItem?.Quantity ?? 0)} left in stock.");
            }
            if (cartItem == null)
            {
                cartItem = mapper.Map<CartItem>(dto);
                cartItem.CartId = cart.Id;
                cartItem.Product = product;
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += dto.Quantity;
            }

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Added/Updated cart item with Product ID {dto.ProductId} for User ID {userId}.");
            return mapper.Map<CartReadOnlyDTO>(cart);
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart == null)
            {
                throw new EntityNotFoundException($"Cart for User ID {userId} not found.");
            }
            cart.CartItems.Clear();
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Cleared cart for User ID {userId}.");
        }

        public async Task<CartReadOnlyDTO?> GetCartByUserIdAsync(int userId)
        {
            var user = await unitOfWork.UserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {userId} not found.");
            }
            var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if(cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                user.Cart = cart;
                await unitOfWork.CartRepository.AddAsync(cart);
                await unitOfWork.SaveChangesAsync();
                logger.LogInformation($"Created new cart for user with ID {userId}.");
            }
            return mapper.Map<CartReadOnlyDTO>(cart);
        }

        public async Task RemoveItemAsync(int userId, int productId)
        {
            var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart == null)
            {
                throw new EntityNotFoundException("Cart not found for the specified user.");
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                throw new EntityNotFoundException($"Cart item with Product ID {productId} for User ID {userId} not found.");
            }
            cart.CartItems.Remove(cartItem);
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Removed cart item with Product ID {productId} for User ID {userId}.");
        }

        public async Task<CartReadOnlyDTO?> UpdateCartItemQuantityAsync(int userId, CartItemUpdateDTO dto)
        {
            var user = await unitOfWork.UserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {userId} not found.");
            }

            var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart == null)
            {
                throw new EntityNotFoundException($"Cart for User ID {userId} not found.");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (cartItem == null)
            {
                throw new EntityNotFoundException($"Cart item with Product ID {dto.ProductId} for User ID {userId} not found.");
            }
            if(dto.Quantity > cartItem.Product.StockQuantity)
            {
                throw new InsufficientStockException($"Cannot update quantity to {dto.Quantity}. Only {cartItem.Product.StockQuantity} left in stock.");
            }

            if (dto.Quantity < 1)
            {
                cart.CartItems.Remove(cartItem);
                await unitOfWork.SaveChangesAsync();
                logger.LogInformation($"Removed cart item with Product ID {dto.ProductId} for User ID {userId}.");
                return mapper.Map<CartReadOnlyDTO>(cart);
            }

            cartItem.Quantity = dto.Quantity;
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Updated quantity for cart item with Product ID {dto.ProductId} for User ID {userId}.");

            return mapper.Map<CartReadOnlyDTO>(cart);
        }
    }
}
