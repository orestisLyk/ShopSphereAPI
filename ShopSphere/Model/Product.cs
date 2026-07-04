namespace ShopSphere.Model
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public string Sku { get; set; } = null!;
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
    }
}
