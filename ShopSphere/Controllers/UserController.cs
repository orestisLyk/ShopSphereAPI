using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.DTO;
using ShopSphere.Service;
using System.Security.Claims;
using ShopSphere.Enums;

namespace ShopSphere.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await userService.RegisterUserAsync(dto);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = created.Id }, created);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var token = await userService.Login(dto.Username, dto.Password);
            return Ok(new { token });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var users = await userService.GetUsersAsync(page, size);
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var currentUserId = GetCurrentUserId();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString() && currentUserId != id)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("by-username/{username}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var currentUserId = GetCurrentUserId();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            // Allow admin or the user themselves
            var target = await userService.GetUserByUsernameAsync(username);
            if (userRole != RoleName.Admin.ToString() && target.Id != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            return Ok(target);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserUpdateDTO dto)
        {
            var currentUserId = GetCurrentUserId();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString() && currentUserId != id)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await userService.UpdateUserAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var currentUserId = GetCurrentUserId();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != RoleName.Admin.ToString() && currentUserId != id)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }
            await userService.DeleteUserAsync(id);
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var id))
            {
                throw new UnauthorizedAccessException("Invalid user token.");
            }
            return id;
        }
    }
}
