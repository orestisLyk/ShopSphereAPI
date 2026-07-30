using ShopSphere.Configuration;
using ShopSphere.DTO;
using ShopSphere.Model;
using Stripe;

namespace ShopSphere.Service
{
    public class StripeService : IStripeService
    {
        private readonly IStripeClient stripeClient;
        private readonly StripeSettings stripeSettings;
        private readonly ILogger<StripeService> logger;

        public StripeService(IStripeClient stripeClient, StripeSettings stripeSettings, ILogger<StripeService> logger)
        {
            this.stripeClient = stripeClient;
            this.stripeSettings = stripeSettings;
            this.logger = logger;
        }
        public async Task CancelPaymentIntentAsync(string paymentIntentId)
        {
            var paymentIntentService = new PaymentIntentService(stripeClient);

            await paymentIntentService.CancelAsync(paymentIntentId);

            logger.LogInformation(
                "Cancelled Stripe PaymentIntent {PaymentIntentId}",
                paymentIntentId);
        }

        public async Task<StripePaymentIntentResult> CreatePaymentIntentAsync(Order order)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = stripeSettings.Currency,

                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                },

                Metadata = new Dictionary<string, string>
            {
                { "OrderId", order.Id.ToString() }
            }
            };

            var paymentIntentService = new PaymentIntentService(stripeClient);

            var paymentIntent = await paymentIntentService.CreateAsync(options);

            logger.LogInformation(
                "Created Stripe PaymentIntent {PaymentIntentId} for Order {OrderId}",
                paymentIntent.Id,
                order.Id);

            return new StripePaymentIntentResult(
                paymentIntent.Id,
                paymentIntent.ClientSecret!
            );
        }
    }
}
