using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record UserUpdateDTO(
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        string? Username,
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        string? Email,
        string? OldPassword,
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters long.")]
        string? NewPassword
    )
    {
    }
}
