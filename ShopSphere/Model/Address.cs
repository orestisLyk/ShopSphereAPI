namespace ShopSphere.Model
{
    public class Address : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Street { get; set; }

        public string StreetNumber { get; set; }

        public bool IsDefaultShipping { get; set; } = false;

        public bool IsDefaultBilling { get; set; } = false;

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
