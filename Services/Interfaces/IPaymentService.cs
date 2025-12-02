using AutoZone.DTOs;

namespace AutoZone.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResponse<CheckoutResponseDto>> CreateCheckoutSessionAsync(CreateCheckoutSessionDto createCheckoutSessionDto);

        Task HandleCheckoutSessionCompletedAsync(string sessionId);
    }
}
