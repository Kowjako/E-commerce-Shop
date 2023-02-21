using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Errors;
using Stripe;

namespace ShopAPI.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentSvc;
        private readonly ILogger<PaymentController> _logger;
        private const string WhSecret = "";

        public PaymentController(IPaymentService paymentSvc, ILogger<PaymentController> logger)
        {
            _paymentSvc = paymentSvc;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentSvc.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
            {
                return BadRequest(new ApiResponse(400, "Problem with your basket"));
            }
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded:", intent.Id);
                    await _paymentSvc.UpdateOrderPaymentSucceeded(intent.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed:", intent.Id);
                    await _paymentSvc.UpdateOrderPaymentFailed(intent.Id);
                    break;
                default:
                    break;
            }

            return new EmptyResult();
        }
    }
}
