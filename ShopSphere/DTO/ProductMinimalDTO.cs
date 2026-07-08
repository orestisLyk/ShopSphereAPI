namespace ShopSphere.DTO
{
    public record ProductMinimalDTO(
        int Id,
        string Sku,
        string Name,
        decimal Price,
        string Description
    )
    {
    }
}
