namespace ShopSphere.DTO
{
    public record ProductDetailsDTO(
        int Id,
        string Sku,
        string Name,
        decimal Price,
        string Description,
        string CategoryName,
        IEnumerable<string> ImageUrls
    )
    {
    }
}
