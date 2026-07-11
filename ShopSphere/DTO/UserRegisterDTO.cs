using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record UserRegisterDTO(
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        string Username,
        [Required(ErrorMessage = "First name is required.")]
        string FirstName,
        [Required(ErrorMessage = "Last name is required.")]
        string LastName,
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        string Email,
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        string Password,
        [Required(ErrorMessage = "Role ID is required.")]
        int RoleId
    )
    {
    }
}
