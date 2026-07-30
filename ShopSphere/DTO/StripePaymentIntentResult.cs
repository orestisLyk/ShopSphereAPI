namespace ShopSphere.DTO
{
    public record StripePaymentIntentResult(
        string ClientSecret,
        string PaymentIntentId
    )
    {
    }
}
