using ShopSphere.Enums;

namespace ShopSphere.DTO
{
    public record OrderReadOnlyDTO(
        int Id,
        decimal TotalAmount,
        OrderStatus Status,
        string Username
        )
    {
    }
}
