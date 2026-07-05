using Microsoft.EntityFrameworkCore;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ShopSphereContext context) : base(context)
        {
        }

        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            return await context.Payments.FirstOrDefaultAsync(p => p.Order.Id == orderId);
        }

        public async Task<bool> PaymentExistsForOrderAsync(int orderId)
        {
            return await context.Payments.AnyAsync(p => p.Order.Id == orderId);
        }
    }
}
