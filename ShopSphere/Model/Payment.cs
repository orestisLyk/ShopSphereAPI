using ShopSphere.Enums;

namespace ShopSphere.Model
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string? ProviderPaymentId { get; set; }
        public DateTime? CompletedAt { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public Order Order { get; set; }
    }
}
