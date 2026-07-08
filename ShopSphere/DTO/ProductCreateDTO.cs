using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record ProductCreateDTO(
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        string Name,
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        decimal Price,
        string? Description,
        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
        int CategoryId
    )
    { }
    
}
