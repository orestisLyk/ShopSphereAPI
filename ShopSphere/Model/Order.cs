using ShopSphere.Enums;

namespace ShopSphere.Model
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
}
