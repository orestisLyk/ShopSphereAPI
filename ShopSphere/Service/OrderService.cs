using AutoMapper;
using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Enums;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<OrderService> logger;
        private readonly IMapper mapper;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }


        public async Task CancelOrderAsync(int orderId, int currentUserId)
        {
            var order = await unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);
            if (order == null)
            {
                throw new EntityNotFoundException($"Order with ID {orderId} not found.");
            }
            if (currentUserId != order.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to cancel this order.");
            }
            if(order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be canceled.");
            }

            if (order.Payment?.PaymentStatus == PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel an order with completed payment.");
            }

            order.Status = OrderStatus.Cancelled;
            foreach (var item in order.OrderItems)
            {
                var product = item.Product;
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                }
            }

            if(order.Payment?.PaymentStatus == PaymentStatus.Pending)
            {
                order.Payment.PaymentStatus = PaymentStatus.Cancelled;
            }

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation($"Order {order.Id} canceled by User {currentUserId}.");
        }

            public async Task<OrderReadOnlyDTO?> CheckoutAsync(int userId)
            {
                var user = await unitOfWork.UserRepository.GetAsync(userId);
                if (user == null)
                {
                    throw new EntityNotFoundException($"User with ID {userId} not found.");
                }

                var cart = await unitOfWork.CartRepository.GetCartWithItemsByUserIdAsync(userId);
                if(cart == null)
                {
                    throw new EntityNotFoundException($"Cart for User ID {userId} not found.");
                }
                if (cart.CartItems == null || !cart.CartItems.Any())
                {
                    throw new InvalidOperationException($"Cart for User ID {userId} is empty.");
                }

                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in cart.CartItems)
                {
                    if (item.Product == null || item.Product.IsDeleted)
                    {
                        throw new InvalidOperationException($"Product with ID {item.ProductId} is not available.");
                    }
                    if (item.Quantity > item.Product.StockQuantity)
                    {
                        throw new InvalidOperationException($"Not enough stock for Product ID {item.ProductId}. Requested: {item.Quantity}, Available: {item.Product.StockQuantity}");
                    }

                    totalAmount += item.Quantity * item.Product.Price;
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };
                    orderItems.Add(orderItem);
                    item.Product.StockQuantity -= item.Quantity;
                }
                

                var order = new Order
                {
                    UserId = userId,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    OrderItems = orderItems,
                    Payment = new Payment
                    {
                        Amount = totalAmount,
                        PaymentStatus = PaymentStatus.Pending,
                    }
                };
                await unitOfWork.OrderRepository.AddAsync(order);
                cart.CartItems.Clear();

                await unitOfWork.SaveChangesAsync();
                logger.LogInformation($"Order {order.Id} created for User {userId} with total amount {totalAmount}.");

            return mapper.Map<OrderReadOnlyDTO>(order);
            }

        public async Task<OrderReadOnlyDTO?> GetOrderByIdAsync(int orderId, int currentUserId, bool isAdmin)
        {
            var order = await unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);
            if(order == null)
            {
                throw new EntityNotFoundException($"Order with ID {orderId} not found.");
            }
            if (!isAdmin && order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view this order.");
            }
            return mapper.Map<OrderReadOnlyDTO>(order);
        }

        public async Task<PaginatedResult<OrderReadOnlyDTO>> GetOrdersByUserAsync(int userId, int currentUserId, bool isAdmin, int pageNumber, int pageSize)
        {
            if (!isAdmin && userId != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to view these orders.");
            }
            var orders = await unitOfWork.OrderRepository.GetOrdersByUserAsync(userId, pageNumber, pageSize);
            return new PaginatedResult<OrderReadOnlyDTO>(
                orders.Items.Select(o => mapper.Map<OrderReadOnlyDTO>(o)).ToList(),
                orders.TotalRecords,
                orders.PageNumber,
                orders.PageSize
            );
        }

        public async Task<OrderReadOnlyDTO?> UpdateOrderStatus(int orderId, OrderStatus status, bool isAdmin)
        {
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only admins can update order status.");
            }

            var order = await unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);
            if (order == null)
            {
                throw new EntityNotFoundException($"Order with ID {orderId} not found.");
            }

            order.Status = status;
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<OrderReadOnlyDTO>(order);
        }
    }
}
