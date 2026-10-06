using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record UserLoginDTO(
        [Required]
        string Username,
        [Required]
        string Password
    )
    {
    }
}
