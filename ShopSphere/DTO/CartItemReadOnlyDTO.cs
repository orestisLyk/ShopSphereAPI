namespace ShopSphere.DTO
{
    public record CartItemReadOnlyDTO(
        int ProductId,
        string ProductName,
        string Sku,
        decimal ProductPrice,
        int Quantity,
        decimal TotalPrice
    )
    {
    }
}
