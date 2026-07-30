using ShopSphere.DTO;
using ShopSphere.Enums;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;

namespace ShopSphere.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStripeService stripeService;
        private readonly ILogger<PaymentService> logger;

        public PaymentService(IUnitOfWork unitOfWork, IStripeService stripeService, ILogger<PaymentService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.stripeService = stripeService;
            this.logger = logger;
        }

        public async Task CancelPaymentIntentAsync(int orderId)
        {
            Order? order = await unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);

            if(order == null)
            {
                throw new EntityNotFoundException($"Order with ID {orderId} not found.");
            }

            var payment = order.Payment;

            if(payment == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} does not have an associated payment.");
            }

            if (payment.PaymentStatus != PaymentStatus.Pending)
            {
                throw new InvalidOperationException($"Payment for order with ID {orderId} is not pending and cannot be canceled.");
            }

            if (payment.ProviderPaymentId == null)
            {
                throw new InvalidOperationException($"Payment for order with ID {orderId} does not have a provider payment ID.");
            }

            await stripeService.CancelPaymentIntentAsync(payment.ProviderPaymentId);

            payment.PaymentStatus = PaymentStatus.Cancelled;
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Cancelled payment for order with ID {OrderId}", orderId);
        }

        public async Task<PaymentIntentReadOnlyDTO> CreatePaymentIntentAsync(int orderId)
        {
            Order? order = await unitOfWork.OrderRepository.GetOrderWithItemsAsync(orderId);

            if(order == null)
            {
                throw new EntityNotFoundException($"Order with ID {orderId} not found.");
            }

            if(order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException($"Order with ID {orderId} is not in a pending state and cannot be paid.");
            }


            var payment = order.Payment;

            if (payment == null)
            {
                throw new InvalidOperationException($"Order with ID {orderId} does not have an associated payment.");
            }

            if (!string.IsNullOrEmpty(payment.ProviderPaymentId))
            {
                throw new InvalidOperationException($"Payment for order with ID {orderId} already has a provider payment ID.");
            }

            var paymentIntentResult = await stripeService.CreatePaymentIntentAsync(order);

            payment.ProviderPaymentId = paymentIntentResult.PaymentIntentId;

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Created payment intent for order with ID {OrderId}", orderId);

            return new PaymentIntentReadOnlyDTO(
                paymentIntentResult.ClientSecret,
                order.Id
            );
        }
    }
}
