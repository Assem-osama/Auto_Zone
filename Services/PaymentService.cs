using AutoZone.DTOs;
using AutoZone.Models;
using AutoZone.Services.Interfaces;
using AutoZone.UnitOfWorks;

namespace AutoZone.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse<CheckoutResponseDto>> CreateCheckoutSessionAsync(CreateCheckoutSessionDto createCheckoutSessionDto)
        {
            try
            {
                var session = await _unitOfWork.PaymentTransactions.CreateCheckoutSessionAsync(createCheckoutSessionDto);

                if (session == null)
                {
                    return ServiceResponse<CheckoutResponseDto>.FailureResponse("Failed to create checkout session");
                }

                var checkoutResponse = new CheckoutResponseDto
                {
                    SessionId = session.Id,
                    PubKey = "", // The public key is usually in settings, but the client might already have it or we can pass it if needed. For now, let's keep it empty or remove it if not needed. Actually, the client needs the session ID mostly.
                    Url = session.Url
                };

                return ServiceResponse<CheckoutResponseDto>.SuccessResponse(checkoutResponse, "Checkout session created successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<CheckoutResponseDto>.FailureResponse($"Error creating checkout session: {ex.Message}");
            }
        }

        /// <summary>
        /// يعالج حدث انتهاء الدفع من Stripe Webhook
        /// </summary>
        /// <param name="sessionId">Stripe Checkout Session ID</param>
        public async Task HandleCheckoutSessionCompletedAsync(string sessionId)
        {

            var transaction = await _unitOfWork.PaymentTransactions.GetBySessionIdAsync(sessionId);

            if (transaction != null)
            {
                transaction.Status = "Paid";
                await _unitOfWork.SaveAsync();
            }
        }

    }
}
