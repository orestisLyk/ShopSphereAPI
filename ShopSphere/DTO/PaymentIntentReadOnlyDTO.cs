namespace ShopSphere.DTO
{
    public record PaymentIntentReadOnlyDTO(
        string ClientSecret,
        int OrderId
    )
    {
    }
}
