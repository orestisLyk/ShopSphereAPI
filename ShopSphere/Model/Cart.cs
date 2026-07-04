namespace ShopSphere.Model
{
    public class Cart : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
