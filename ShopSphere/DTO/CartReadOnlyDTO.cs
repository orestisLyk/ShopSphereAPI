namespace ShopSphere.DTO
{
    public record CartReadOnlyDTO(
        int Id,
        int UserId,
        IEnumerable<CartItemReadOnlyDTO> CartItems,
        decimal TotalPrice,
        int TotalQuantity
        )
    {
    }
}
