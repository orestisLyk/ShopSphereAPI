namespace ShopSphere.DTO
{
    public record ProductImageReadDTO(
        int Id,
        string ImageUrl,
        int ProductId
    )
    {
    }
}
