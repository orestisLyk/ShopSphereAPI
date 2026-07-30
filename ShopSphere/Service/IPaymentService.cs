using ShopSphere.DTO;

namespace ShopSphere.Service
{
    public interface IPaymentService
    {
        Task<PaymentIntentReadOnlyDTO> CreatePaymentIntentAsync(int orderId);
        Task CancelPaymentIntentAsync(int orderId);
    }
}
