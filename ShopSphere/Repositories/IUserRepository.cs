using ShopSphere.Core;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize);
    }
}
