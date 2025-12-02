using AutoZone.DTOs;
using AutoZone.Models;
using AutoZone.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.Annotations;


namespace AutoZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IPaymentService _paymentService;
        private readonly string _stripeWebhookSecret;
        public PaymentController(IPaymentService paymentService, IOptions<StripeSettings> stripeSettings)
        {
            _paymentService = paymentService;
            _stripeWebhookSecret = stripeSettings.Value.WebhookSecret;
        }

        /// <summary>
        /// Create Checkout Session
        /// </summary>
        /// <param name="parameters">Filtering and sorting parameters</param>
        /// <returns>A paged list of cars</returns>
        /// <response code="200">List of cars retrieved successfully</response>
        [SwaggerOperation(Summary = "Get all cars", Description = "Returns a paged list of cars with optional filtering and sorting.")]
        [SwaggerResponse(200, "List of cars retrieved successfully")]
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionDto createCheckoutSessionDto)
        {
            var sessionID = await _paymentService.CreateCheckoutSessionAsync(createCheckoutSessionDto);
            if (!sessionID.Success)
            {
                return BadRequest("Failed to create checkout session");
            }
            return Ok(sessionID);
        }





        /// <summary>
        /// Stripe Webhook Handler
        /// </summary>
        [HttpPost("webhook")]
            
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeWebhookSecret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null)
                    {
                        // هنا بنحدّث حالة الدفع في قاعدة البيانات
                        await _paymentService.HandleCheckoutSessionCompletedAsync(session.Id);
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest($"Stripe webhook error: {e.Message}");
            }
        }
    }
}
