namespace ShopSphere.Model
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Cart Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
