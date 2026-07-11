using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ShopSphere.Core;
using ShopSphere.DTO;
using ShopSphere.Exceptions;
using ShopSphere.Model;
using ShopSphere.Repositories;
using ShopSphere.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopSphere.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<UserService> logger;
        private readonly IEncryptionUtil encryptionUtil;
        private readonly IConfiguration configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IEncryptionUtil encryptionUtil, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.encryptionUtil = encryptionUtil;
            this.configuration = configuration;
        }

        public async Task<UserReadOnlyDTO> RegisterUserAsync(UserRegisterDTO dto)
        {
            if (await unitOfWork.UserRepository.UsernameExistsAsync(dto.Username))
            {
                throw new EntityAlreadyExistsException($"Username '{dto.Username}' is already taken.");
            }
            if (await unitOfWork.UserRepository.EmailExistsAsync(dto.Email))
            {
                throw new EntityAlreadyExistsException($"Email '{dto.Email}' is already taken.");
            }
            var user = mapper.Map<User>(dto);
            user.HashedPassword = encryptionUtil.Encrypt(dto.Password);
            await unitOfWork.UserRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<PaginatedResult<UserReadOnlyDTO>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var paginatedUsers = await unitOfWork.UserRepository.GetUsersAsync(pageNumber, pageSize);
            var userDTOs = mapper.Map<List<UserReadOnlyDTO>>(paginatedUsers.Items);
            return new PaginatedResult<UserReadOnlyDTO>
            {
                Items = userDTOs,
                TotalRecords = paginatedUsers.TotalRecords,
                PageNumber = paginatedUsers.PageNumber,
                PageSize = paginatedUsers.PageSize
            };
        }

        public async Task<UserReadOnlyDTO> GetUserByUsernameAsync(string username)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with username '{username}' not found.");
            }
            return mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<UserReadOnlyDTO> GetUserByIdAsync(int id)
        {
            var user = await unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID '{id}' not found.");
            }
            return mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task<UserReadOnlyDTO> UpdateUserAsync(int id, UserUpdateDTO dto)
        {
            var user = await unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID '{id}' not found.");
            }
            mapper.Map(dto, user);
            if (!string.IsNullOrEmpty(dto.OldPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                if (encryptionUtil.Verify(dto.OldPassword, user.HashedPassword))
                {
                    user.HashedPassword = encryptionUtil.Encrypt(dto.NewPassword);
                }
                else
                {
                    throw new InvalidCredentialsException("Old password is incorrect.");
                }
            }

            await unitOfWork.SaveChangesAsync();
            return mapper.Map<UserReadOnlyDTO>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID '{id}' not found.");
            }
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with username '{username}' not found.");
            }

            if (!encryptionUtil.Verify(password, user.HashedPassword))
            {
                throw new InvalidCredentialsException("Invalid username or password.");
            }

            return CreateUserToken(user);
        }

        private string CreateUserToken(User user)
        {
            var appSecurityKey = configuration["Jwt:Secret"];
            if(appSecurityKey == null)
            {
                throw new ConfigurationException("JWT secret key is not configured. Please check your application settings.");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),

            };

            var capabilities = user.Role.Capabilities;

            foreach (var capability in capabilities)
            {
                claimsInfo.Add(new Claim("capability", capability.Name));
            }

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claimsInfo,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
            );

            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken;
        }
    }
}
