using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Model;

namespace ShopSphere.Service
{
    public interface IUserService
    {
        Task<UserReadOnlyDTO> RegisterUserAsync(UserRegisterDTO dto);
        Task<PaginatedResult<UserReadOnlyDTO>> GetUsersAsync(int pageNumber, int pageSize);
        Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username);
        Task<UserReadOnlyDTO> GetUserByIdAsync(int id);
        Task<UserReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO dto);
        Task DeleteUserAsync(int id);
        Task<string> Login(string username, string password);
    }
}
