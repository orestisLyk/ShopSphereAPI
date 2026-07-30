using ShopSphere.DTO;
using ShopSphere.Migrations;
using ShopSphere.Model;

namespace ShopSphere.Service
{
    public interface IStripeService
    {
        Task<StripePaymentIntentResult> CreatePaymentIntentAsync(Order order);

        Task CancelPaymentIntentAsync(string paymentIntentId);
    }
}
