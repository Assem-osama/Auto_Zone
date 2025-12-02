using System.Threading.Tasks;
using AutoZone.DTOs;
using AutoZone.Models;
using AutoZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace AutoZone.Repositories
{
    public class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly AutoZonedbContext _context;
        private readonly StripeSettings _stripeSettings;
        public PaymentTransactionRepository(AutoZonedbContext context, IOptions<StripeSettings> stripeSettings) : base(context)
        {
            _context = context;
            _stripeSettings = stripeSettings.Value;
            Stripe.StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<Stripe.Checkout.Session> CreateCheckoutSessionAsync(CreateCheckoutSessionDto createCheckoutSessionDto)
        {
            // Implementation for creating a checkout session

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment", // or "subscription"
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = createCheckoutSessionDto.Amount * 100, // Stripe expects amount in cents
                            Currency = createCheckoutSessionDto.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Car #{createCheckoutSessionDto.CarId} payment"
                            }
                        },
                        Quantity = 1
                    }
                },
                SuccessUrl = createCheckoutSessionDto.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = createCheckoutSessionDto.CancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", createCheckoutSessionDto.UserId.ToString() },
                    { "CarId", createCheckoutSessionDto.CarId.ToString() }
                }
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);

            var paymentTransaction = new PaymentTransaction
            {
                UserId = createCheckoutSessionDto.UserId,
                CarId = createCheckoutSessionDto.CarId,
                Amount = createCheckoutSessionDto.Amount,
                Currency = createCheckoutSessionDto.Currency,
                StripeSessionId = session.Id,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.PaymentTransactions.Add(paymentTransaction);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<PaymentTransaction> GetBySessionIdAsync(string sessionId)
        {
            return await _context.PaymentTransactions
                .FirstOrDefaultAsync(pt => pt.StripeSessionId == sessionId);
        }
    }
}
