using System.Threading.Tasks;
using AutoZone.DTOs;
using AutoZone.Models;

using Stripe.Checkout;

namespace AutoZone.Repositories.Interfaces
{
    public interface IPaymentTransactionRepository:IGenericRepository<PaymentTransaction>
    {
        Task<Session> CreateCheckoutSessionAsync(CreateCheckoutSessionDto createCheckoutSessionDto);

        Task<PaymentTransaction> GetBySessionIdAsync(string sessionId);
    }
}
