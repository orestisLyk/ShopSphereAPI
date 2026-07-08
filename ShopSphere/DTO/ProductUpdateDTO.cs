using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record ProductUpdateDTO(
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        string Name,
        [Required(ErrorMessage = "Price is Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        decimal Price,
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        string? Description,
        [Required(ErrorMessage = "CategoryId is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer")]
        int CategoryId
        )
    {
        
    }
}
