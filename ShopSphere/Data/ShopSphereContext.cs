using Microsoft.EntityFrameworkCore;
using ShopSphere.Model;

namespace ShopSphere.Data
{
    public class ShopSphereContext : DbContext
    {
        public ShopSphereContext(DbContextOptions<ShopSphereContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Capability> Capabilities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Cart)
                      .WithOne(c => c.User)
                      .HasForeignKey<Cart>(c => c.UserId);

                entity.HasMany(u => u.Orders)
                        .WithOne(o => o.User)
                        .HasForeignKey(o => o.UserId);

                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.HashedPassword).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);


            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(p => p.CartItems)
                      .WithOne(ci => ci.Product)
                      .HasForeignKey(ci => ci.ProductId);
                entity.HasMany(p => p.OrderItems)
                      .WithOne(oi => oi.Product)
                      .HasForeignKey(oi => oi.ProductId);
                entity.HasMany(p => p.ProductImages)
                      .WithOne(pi => pi.Product)
                      .HasForeignKey(pi => pi.ProductId);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasMany(c => c.CartItems)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);

                entity.Property(o => o.Status).IsRequired().HasConversion<string>();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.Order)
                      .WithOne(o => o.Payment)
                      .HasForeignKey<Order>(o => o.PaymentId);

                entity.Property(p => p.PaymentStatus).IsRequired().HasConversion<string>();
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(p => p.ImageUrl).IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.Capabilities)
                      .WithMany(c => c.Roles)
                      .UsingEntity(j => j.ToTable("RolesCapabilities"));
                
                entity.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<Capability>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(c => c.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => new { ci.CartId, ci.ProductId });

                entity.Property(ci => ci.Quantity).IsRequired();

                entity.ToTable(t =>
                    t.HasCheckConstraint("CK_CartItem_Quantity", "\"Quantity\" > 0"));
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(oi => oi.Quantity).IsRequired();
                entity.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
            });
        }
    }
}
