using ShopSphere.Enums;

namespace ShopSphere.Model
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public Order Order { get; set; }
    }
}
