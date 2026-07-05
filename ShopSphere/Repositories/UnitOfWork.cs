using ShopSphere.Data;

namespace ShopSphere.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopSphereContext context;

        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IProductImageRepository ProductImageRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IPaymentRepository PaymentRepository { get; }

        public UnitOfWork(ShopSphereContext context)
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            ProductRepository = new ProductRepository(context);
            ProductImageRepository = new ProductImageRepository(context);
            CategoryRepository = new CategoryRepository(context);
            CartRepository = new CartRepository(context);
            OrderRepository = new OrderRepository(context);
            PaymentRepository = new PaymentRepository(context);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
