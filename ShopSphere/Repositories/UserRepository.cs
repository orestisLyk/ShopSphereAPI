using Microsoft.EntityFrameworkCore;
using ShopSphere.Core;
using ShopSphere.Data;
using ShopSphere.Model;

namespace ShopSphere.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ShopSphereContext context) : base(context)
        {
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            return await context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Capabilities)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var totalCount = await context.Users.CountAsync();

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var users = await context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedResult<User>
            {
                Items = users,
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
