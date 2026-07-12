namespace ShopSphere.DTO
{
    public record CartItemCreateDTO(
        int ProductId,
        int Quantity
    )
    {
    }
}
