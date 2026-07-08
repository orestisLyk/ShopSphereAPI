using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record CategoryCreateDTO(
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        string Name,
        [Required(ErrorMessage = "Category description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        string Description
        )
    {
    }
}
