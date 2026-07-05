using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
        Task<bool> PaymentExistsForOrderAsync(int orderId);
    }
}
