using System.ComponentModel.DataAnnotations;

namespace ShopSphere.DTO
{
    public record ProductImageCreateDTO(
        [Required(ErrorMessage = "Image file is required.")]
        IFormFile ImageFile
        )
    {

    }
}
