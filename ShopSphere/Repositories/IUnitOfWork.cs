namespace ShopSphere.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductImageRepository ProductImageRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPaymentRepository PaymentRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
