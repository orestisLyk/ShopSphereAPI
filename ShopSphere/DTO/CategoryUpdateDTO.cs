using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record CategoryUpdateDTO(
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        string Name,
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        string Description
        )
    {
    }
}
